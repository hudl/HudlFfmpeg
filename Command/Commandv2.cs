using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Command.Managers;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using log4net;

namespace Hudl.Ffmpeg.Command
{
    public class Commandv2
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Commandv2).Name);

        private Commandv2(CommandFactory owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;
            Id = Guid.NewGuid().ToString();
            Objects = CommandObjects.Create(this);
            OutputManager = Commandv2OutputManager.Create(this);
            ResourceManager = Commandv2ResourceManager.Create(this);
            FilterchainManager = Commandv2FilterchainManager.Create(this);
        }

        public static Commandv2 Create(CommandFactory owner)
        {
            return new Commandv2(owner);    
        }

        internal CommandObjects Objects { get; set; }

        public ReadOnlyCollection<CommandOutput> Outputs { get { return Objects.Outputs.AsReadOnly(); } }

        public ReadOnlyCollection<CommandResourcev2> Resources { get { return Objects.Inputs.AsReadOnly(); } }

        public ReadOnlyCollection<Filterchainv2> Filtergraph { get { return Objects.Filtergraph.FilterchainList.AsReadOnly(); } }

        public Commandv2OutputManager OutputManager { get; set; }

        public Commandv2ResourceManager ResourceManager { get; set; }

        public Commandv2FilterchainManager FilterchainManager { get; set; }

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
            where TProcessor : ICommandProcessor, new()
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
            where TProcessor : ICommandProcessor
        {
            if (commandProcessor == null)
            {
                throw new ArgumentNullException("commandProcessor");
            }

            var commandBuilder = new CommandBuilder();
            commandBuilder.WriteCommand(this);
            
            if (!commandProcessor.Send(commandBuilder.ToString()))
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return Objects.Outputs;
        }

        #region Internals
        internal string Id { get; set; }
        internal CommandFactory Owner { get; set; }

        internal List<CommandResourcev2> ResourcesFromReceipts(params CommandReceipt[] receipts)
        {
            return ResourcesFromReceipts(new List<CommandReceipt>(receipts));
        }
        internal List<CommandResourcev2> ResourcesFromReceipts(List<CommandReceipt> receipts)
        {
            if (receipts == null || receipts.Count == 0)
            {
                throw new ArgumentException("Receipts cannot be null or empty for a retrieve.", "receipts");
            }

            return receipts.Select(receipt =>
                {
                    CommandResourcev2 resource = null;
                    switch (receipt.Type)
                    {
                        case CommandReceiptType.Input:
                            resource = Objects.Inputs.FirstOrDefault(r => r.Resource.Map == receipt.Map);
                            break;
                        case CommandReceiptType.Stream:
                            var filterchain = Objects.Filtergraph.FilterchainList.FirstOrDefault(f => f.GetReceipts().Any(r => r.Equals(receipt)));
                            var filterchainOutput = filterchain.Outputs(this).FirstOrDefault(r => r.Resource.Map == receipt.Map);
                            resource = CommandResourcev2.Create(filterchainOutput.Resource);
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

        internal Filterchainv2 FilterchainFromReceipt(CommandReceipt receipt)
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
