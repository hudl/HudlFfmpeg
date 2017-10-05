using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Exceptions;
using System;
using System.Threading;

namespace Hudl.FFmpeg.Command.Models
{
    public abstract class FFCommandBase : ICommand
    {
        protected FFCommandBase()
        {
            PreExecutionAction = EmptyOperation;
            PostExecutionAction = EmptyOperation;
            OnSuccessAction = EmptyOperation;
            OnErrorAction = EmptyOperation;
        }

        public ICommandFactory Owner { get; protected set; }

        public Action<ICommandFactory, ICommand, bool> PreExecutionAction { get; set; }

        public Action<ICommandFactory, ICommand, bool> PostExecutionAction { get; set; }

        public Action<ICommandFactory, ICommand, ICommandProcessor> OnSuccessAction { get; set; }

        public Action<ICommandFactory, ICommand, ICommandProcessor> OnErrorAction { get; set; }

        public ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>()
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new()
        {
            return ExecuteWith<TProcessorType, TBuilderType>(null);
        }

        public ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(int? timeoutMilliseconds)
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            if (timeoutMilliseconds.HasValue)
            {
                cancellationTokenSource.CancelAfter(timeoutMilliseconds.Value);
            }

            return ExecuteWith<TProcessorType, TBuilderType>(cancellationTokenSource.Token);
        }

        public ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(CancellationToken token = default(CancellationToken))
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new()
        {
            var commandProcessor = new TProcessorType();

            if (!commandProcessor.Open())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            var returnType = ExecuteWith<TProcessorType, TBuilderType>(commandProcessor, token);

            if (!commandProcessor.Close())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        public ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(TProcessorType commandProcessor, CancellationToken token = default(CancellationToken))
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

            if (!commandProcessor.Send(commandBuilder.ToString(), token))
            {
                PostExecutionAction(Owner, this, false);

                OnErrorAction(Owner, this, commandProcessor);

                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            PostExecutionAction(Owner, this, true);

            OnSuccessAction(Owner, this, commandProcessor);

            return commandProcessor;
        }

        internal void EmptyOperation(ICommandFactory factory, ICommand command, bool success)
        {
        }
        internal void EmptyOperation(ICommandFactory factory, ICommand command, ICommandProcessor processor)
        {
        }
    }
}
