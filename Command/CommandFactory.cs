using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandFactory
    {
        private readonly List<Command<IResource>> _commandList = new List<Command<IResource>>();

        public Command<TOutput> OutputAs<TOutput>()
            where TOutput : IResource, new()
        {
            return OutputAs(new Command<TOutput>());
        }

        public Command<TOutput> OutputAs<TOutput>(Command<TOutput> command)
            where TOutput : IResource, new()
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            _commandList.Add(command);
            return command;
        }
    }
}
