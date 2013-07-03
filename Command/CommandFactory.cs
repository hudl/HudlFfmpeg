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
            ExclusionList = new List<string>();
            CommandList = new List<Command<IResource>>();
        }

        /// <summary>
        /// Adds a new command to the CommandFactory
        /// </summary>
        public Command<TOutput> OutputAs<TOutput>()
            where TOutput : IResource, new()
        {
            return OutputAs(new Command<TOutput>(this), true);
        }

        /// <summary>
        /// Adds a new command to the CommandFactory
        /// </summary>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public Command<TOutput> OutputAs<TOutput>(bool export)
            where TOutput : IResource, new()
        {
            return OutputAs(new Command<TOutput>(this), export);
        }

        /// <summary>
        /// Adds a new command to the CommandFactory
        /// </summary>
        public Command<TOutput> OutputAs<TOutput>(Command<TOutput> command)
            where TOutput : IResource, new()
        {
            return OutputAs(command, true);
        }

        /// <summary>
        /// Adds the new command to the CommandFactory
        /// </summary>
        /// <param name="command">The command to be entered into the factory</param>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public Command<TOutput> OutputAs<TOutput>(Command<TOutput> command, bool export)
            where TOutput : IResource, new()
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (Contains(command))
            {
                throw new ArgumentException("Command is already a part of this collection.");
            }

            command.Parent = this;
            CommandList.Add(command as Command<IResource>);
            if (!export)
            {
                ExclusionList.Add(command.Id);
            }
            return command;
        }

        /// <summary>
        /// Select the output resources for the current command factory 
        /// </summary>
        /// <returns></returns>
        public List<IResource> GetOutput()
        {
            return CommandList.Where(c => !ExclusionList.Contains(c.Id))
                              .Select(c => c.Output.Resource).ToList();
        }

        public int Count { get { return CommandList.Count; } }

        public bool Contains<TOutput>(Command<TOutput> command)
            where TOutput : IResource
        {
            return (CommandList.Count(c => c.Id == command.Id) > 0);
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
        internal List<string> ExclusionList { get; set; }
        internal List<Command<IResource>> CommandList { get; set; }
        #endregion
    }
}
