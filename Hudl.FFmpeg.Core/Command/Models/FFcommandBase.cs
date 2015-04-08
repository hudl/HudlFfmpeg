using System;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Command.BaseTypes;

namespace Hudl.FFmpeg.Command.Models
{
    public abstract class FFcommandBase : ICommand
    {
        protected FFcommandBase()
        {
            PreExecutionAction = EmptyOperation;
            PostExecutionAction = EmptyOperation;
        }

        public ICommandFactory Owner { get; protected set; }

        public Action<ICommandFactory, ICommand, bool> PreExecutionAction { get; set; }

        public Action<ICommandFactory, ICommand, bool> PostExecutionAction { get; set; }

        public ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>()
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new()
        {
            var commandProcessor = new TProcessorType();

            if (!commandProcessor.Open())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            var returnType = ExecuteWith<TProcessorType, TBuilderType>(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        public ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(TProcessorType commandProcessor)
            where TProcessorType : class, ICommandProcessor
            where TBuilderType : class, ICommandBuilder, new()
        {
            if (commandProcessor == null)
            {
                throw new ArgumentNullException("commandProcessor");
            }

            var commandBuilder = new TBuilderType();
            commandBuilder.WriteCommand(this);

            PreExecutionAction(Owner, this, true);

            if (!commandProcessor.Send(commandBuilder.ToString()))
            {
                PostExecutionAction(Owner, this, false);

                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            PostExecutionAction(Owner, this, true);

            return commandProcessor;
        }

        internal void EmptyOperation(ICommandFactory factory, ICommand command, bool success)
        {
        }
    }
}
