using System;
using System.Collections.ObjectModel;
using System.Deployment.Internal;
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
            InputManager = CommandInputManager.Create(this);
            FilterchainManager = CommandFiltergraphManager.Create(this);
            PreRenderAction = EmptyOperation;
            PostRenderAction = EmptyOperation;
        }

        public static FfmpegCommand Create(CommandFactory owner)
        {
            return new FfmpegCommand(owner);    
        }

        public Action<CommandFactory, FfmpegCommand, bool> PreRenderAction { get; set; }

        public Action<CommandFactory, FfmpegCommand, bool> PostRenderAction { get; set; }

        public ReadOnlyCollection<CommandOutput> Outputs { get { return Objects.Outputs.AsReadOnly(); } }

        public ReadOnlyCollection<CommandInput> Inputs { get { return Objects.Inputs.AsReadOnly(); } }

        public ReadOnlyCollection<Filterchain> Filtergraph { get { return Objects.Filtergraph.FilterchainList.AsReadOnly(); } }

        public CommandOutputManager OutputManager { get; set; }

        public CommandInputManager InputManager { get; set; }

        public CommandFiltergraphManager FilterchainManager { get; set; }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<CommandOutput> Render()
        {
            return RenderWith<FfmpegProcessorReciever>();
        }

        #region Internals
        internal string Id { get; set; }

        internal CommandObjects Objects { get; set; }

        internal CommandFactory Owner { get; set; }

        internal CommandInput ResourceFromReceipt(CommandReceipt receipt)
        {
            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }

            return Objects.Inputs.FirstOrDefault(i => i.GetReceipt().Map == receipt.Map); 
        }

        internal CommandOutput OutputFromReceipt(CommandReceipt receipt)
        {
            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }

            return Objects.Outputs.FirstOrDefault(i => i.GetReceipt().Map == receipt.Map); 
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

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        internal List<CommandOutput> RenderWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
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
        internal List<CommandOutput> RenderWith<TProcessor>(TProcessor commandProcessor)
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

        internal void EmptyOperation(CommandFactory factory, FfmpegCommand command, bool success)
        {
        }
        #endregion
    }
}
