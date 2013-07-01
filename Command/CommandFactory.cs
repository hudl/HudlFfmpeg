using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandFactory
    {
        public CommandFactory()
        {
            Id = Guid.NewGuid().ToString();
            CommandList = new List<Command<IResource>>();
        }

        /// <summary>
        /// Adds a new command to the CommandFactory
        /// </summary>
        public Command<TOutput> OutputAs<TOutput>()
            where TOutput : IResource, new()
        {
            return OutputAs(new Command<TOutput>(this));
        }

        /// <summary>
        /// Adds the new command to the CommandFactory
        /// </summary>
        public Command<TOutput> OutputAs<TOutput>(Command<TOutput> command)
            where TOutput : IResource, new()
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.Parent = this;
            CommandList.Add(command);
            return command;
        }


        #region Internals
        internal string Id { get; set; }
        internal List<Command<IResource>> CommandList { get; set; } 
        internal List<IResource> GetOutputList()
        {
            return CommandList.Select(c => c.Output.Resource).ToList();
        }
        #endregion
    }
}
