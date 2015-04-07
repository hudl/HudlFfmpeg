using System;
using Hudl.FFmpeg.BaseTypes;

namespace Hudl.FFmpeg.Command.BaseTypes
{
    public abstract class FFCommandBase
    {
        protected FFCommandBase()
        {
            PreExecutionAction = EmptyOperation;
            PostExecutionAction = EmptyOperation;
        }

        internal CommandFactory Owner { get; set; }

        public Action<CommandFactory, FFCommandBase, bool> PreExecutionAction { get; set; }

        public Action<CommandFactory, FFCommandBase, bool> PostExecutionAction { get; set; }

        internal ICommandProcessor ExecuteWith<TProcessorType>()
            where TProcessorType : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessorType();

            if (!commandProcessor.Open())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            var returnType = ExecuteWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        internal ICommandProcessor ExecuteWith<TProcessorType>(TProcessorType commandProcessor)
            where TProcessorType : class, ICommandProcessor
        {
            if (commandProcessor == null)
            {
                throw new ArgumentNullException("commandProcessor");
            }

            var commandBuilder = new CommandBuilder();
            commandBuilder.WriteCommand((dynamic)this);

            PreExecutionAction(Owner, this, true);

            if (!commandProcessor.Send(commandBuilder.ToString()))
            {
                PostExecutionAction(Owner, this, false);

                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            PostExecutionAction(Owner, this, true);

            return commandProcessor;
        }

        internal void EmptyOperation(CommandFactory factory, FFCommandBase command, bool success)
        {
        }
    }
}
