using System;
using System.Threading;

namespace Hudl.FFmpeg.Command.BaseTypes
{
    public interface ICommand
    {
        ICommandFactory Owner { get; }

        Action<ICommandFactory, ICommand, bool> PreExecutionAction { get; set; }

        Action<ICommandFactory, ICommand, bool> PostExecutionAction { get; set; }

        Action<ICommandFactory, ICommand, ICommandProcessor> OnSuccessAction { get; set; }

        Action<ICommandFactory, ICommand, ICommandProcessor> OnErrorAction { get; set; }

        ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>()
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new();

        ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(CancellationToken token = default(CancellationToken))
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new();

        ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(TProcessorType commandProcessor, CancellationToken token = default(CancellationToken))
            where TProcessorType : class, ICommandProcessor
            where TBuilderType : class, ICommandBuilder, new();
    }
}
