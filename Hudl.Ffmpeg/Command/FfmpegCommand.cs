using System;
using System.Collections.ObjectModel;
using System.Deployment.Internal;
using System.Linq;
using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Managers;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    public class FFmpegCommand
    {
        private FFmpegCommand(CommandFactory owner)
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

        public static FFmpegCommand Create(CommandFactory owner)
        {
            return new FFmpegCommand(owner);    
        }

        public Action<CommandFactory, FFmpegCommand, bool> PreRenderAction { get; set; }

        public Action<CommandFactory, FFmpegCommand, bool> PostRenderAction { get; set; }

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
            return RenderWith<FFmpegProcessorReciever>();
        }

        #region Internals
        internal string Id { get; set; }

        internal CommandObjects Objects { get; set; }

        internal CommandFactory Owner { get; set; }

        

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        internal List<CommandOutput> RenderWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
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

                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            PostRenderAction(Owner, this, true);

            return Objects.Outputs;
        }

        internal void EmptyOperation(CommandFactory factory, FFmpegCommand command, bool success)
        {
        }
        #endregion
    }
}
