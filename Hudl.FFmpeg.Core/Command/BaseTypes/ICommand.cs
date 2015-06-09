using System;

namespace Hudl.FFmpeg.Command.BaseTypes
{
    public interface ICommand
    {
        ICommandFactory Owner { get; }

        Action<ICommandFactory, ICommand, bool> PostExecutionAction { get; set; }

        ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>()
            where TProcessorType : class, ICommandProcessor, new()
            where TBuilderType : class, ICommandBuilder, new();

        ICommandProcessor ExecuteWith<TProcessorType, TBuilderType>(TProcessorType commandProcessor)
            where TProcessorType : class, ICommandProcessor
            where TBuilderType : class, ICommandBuilder, new();
    }
}
