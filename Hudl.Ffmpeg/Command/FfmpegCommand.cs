using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Command.Managers;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class FfmpegCommand
    {
        private FfmpegCommand(CommandFactory owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;
            Id = Guid.NewGuid().ToString();
            Objects = CommandObjects.Create(this);
            OutputManager = CommandOutputManager.Create(this);
            ResourceManager = CommandResourceManager.Create(this);
            FilterchainManager = CommandFilterchainManager.Create(this);
            PreRenderAction = EmptyOperation;
            PostRenderAction = EmptyOperation;
        }

        public static FfmpegCommand Create(CommandFactory owner)
        {
            return new FfmpegCommand(owner);    
        }

        internal CommandObjects Objects { get; set; }

        public Action<CommandFactory, FfmpegCommand, bool> PreRenderAction { get; set; }

        public Action<CommandFactory, FfmpegCommand, bool> PostRenderAction { get; set; }

        public ReadOnlyCollection<CommandOutput> Outputs { get { return Objects.Outputs.AsReadOnly(); } }

        public ReadOnlyCollection<CommandResource> Resources { get { return Objects.Inputs.AsReadOnly(); } }

        public ReadOnlyCollection<Filterchain> Filtergraph { get { return Objects.Filtergraph.FilterchainList.AsReadOnly(); } }

        public CommandOutputManager OutputManager { get; set; }

        public CommandResourceManager ResourceManager { get; set; }

        public CommandFilterchainManager FilterchainManager { get; set; }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<CommandOutput> Render()
        {
            return RenderWith<CmdProcessorReciever>();
        }

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        public List<CommandOutput> RenderWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open(Owner.Configuration))
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        /// <summary>
        /// Renders the command stream with an existing command processor
        /// </summary>
        public List<CommandOutput> RenderWith<TProcessor>(TProcessor commandProcessor)
            where TProcessor : class, ICommandProcessor
        {
            if (commandProcessor == null)
            {
                throw new ArgumentNullException("commandProcessor");
            }

            var commandBuilder = new CommandBuilder();
            commandBuilder.WriteCommand(this);

            PreRenderAction(Owner, this, true);
            
            if (!commandProcessor.Send(commandBuilder.ToString()))
            {
                PostRenderAction(Owner, this, false);

                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            PostRenderAction(Owner, this, true);

            return Objects.Outputs;
        }

        private void EmptyOperation(CommandFactory factory, FfmpegCommand command, bool success)
        {
        }

        #region Internals
        internal string Id { get; set; }
        internal CommandFactory Owner { get; set; }

        internal List<CommandResource> ResourcesFromReceipts(params CommandReceipt[] receipts)
        {
            return ResourcesFromReceipts(new List<CommandReceipt>(receipts));
        }
        internal List<CommandResource> ResourcesFromReceipts(List<CommandReceipt> receipts)
        {
            if (receipts == null || receipts.Count == 0)
            {
                throw new ArgumentException("Receipts cannot be null or empty for a retrieve.", "receipts");
            }

            return receipts.Select(receipt =>
                {
                    CommandResource resource = null;
                    switch (receipt.Type)
                    {
                        case CommandReceiptType.Input:
                            resource = Objects.Inputs.FirstOrDefault(r => r.Resource.Map == receipt.Map);
                            break;
                        case CommandReceiptType.Stream:
                            var filterchain = Objects.Filtergraph.FilterchainList.FirstOrDefault(f => f.GetReceipts().Any(r => r.Equals(receipt)));
                            var filterchainOutput = filterchain.OutputList.FirstOrDefault(r => r.Resource.Map == receipt.Map);
                            resource = CommandResource.Create(filterchainOutput.Resource);
                            resource.Id = filterchainOutput.Id; 
                            resource.Owner = this;
                            break;
                    }
                    if (resource == null)
                    {
                        throw new KeyNotFoundException("Resource was not found in the command list.");
                    }
                    return resource; 
                }).ToList();
        }

        internal Filterchain FilterchainFromReceipt(CommandReceipt receipt)
        {
            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }

            return Objects.Filtergraph.FilterchainList.FirstOrDefault(f => f.GetReceipts().Any(r => r.Equals(receipt)));
        }

        internal CommandReceipt RegenerateResourceMap(CommandReceipt receipt)
        {
            if (receipt.FactoryId != Owner.Id ||
                receipt.CommandId != Id)
            {
                throw new InvalidOperationException("Receipt is not a part of this command.");
            }

            var resource =  Objects.Inputs.FirstOrDefault(r => r.Resource.Map == receipt.Map);
            if (resource == null)
            {
                throw new InvalidOperationException("Receipt is not a part of this command.");
            }

            resource.Resource.Map = Helpers.NewMap();

            return resource.GetReceipt();
        }
        #endregion
    }
}
