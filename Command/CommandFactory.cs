using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
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
            CommandList.Add(command as Command<IResource>);
            return command;
        }

        /// <summary>
        /// Select the output resources for the current command factory 
        /// </summary>
        /// <returns></returns>
        public List<IResource> GetOutput()
        {
            return CommandList.Select(c => c.Output.Resource).ToList();
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IResource> Render()
        {
            return RenderWith<BatchCommandProcessorReciever>();
        }

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
            {
                throw commandProcessor.Error;
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw commandProcessor.Error;
            }

            return returnType;
        }

        /// <summary>
        /// Renders the command stream with an existing command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : ICommandProcessor
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            return CommandList.Select(command =>
                {
                    var output = command.RenderWith(processor);
                    return output.ExportResource 
                        ? output.Output() 
                        : null;
                }).ToList();
        }

        #region Internals
        internal string Id { get; set; }
        internal List<Command<IResource>> CommandList { get; set; } 
        #endregion
    }
}
