using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandFactory
    {
        public CommandFactory()
        {
            Id = Guid.NewGuid().ToString();
            CommandList = new List<Command<IResource>>();
        }

        #region Command Helper Visability
        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>()
            where TOutput : IResource, new()
        {
            return CreateOutput(new TOutput(), true);
        }

        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public Command<TOutput> CreateOutput<TOutput>(bool export)
            where TOutput : IResource, new()
        {
            return CreateOutput(new TOutput(), export);
        }

        /// <summary>
        /// Adds a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        public Command<TOutput> CreateOutput<TOutput>(TOutput outputToUse)
            where TOutput : IResource, new()
        {
            return CreateOutput(outputToUse, true);
        }

        /// <summary>
        /// Adds a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public Command<TOutput> CreateOutput<TOutput>(TOutput outputToUse, bool export)
            where TOutput : IResource, new()
        {
            return CreateOutput(outputToUse, SettingsCollection.ForOutput(), export);
        }

        /// <summary>
        /// Adds a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        /// <param name="outputSettings">The output settings to use for the command.</param>
        public Command<TOutput> CreateOutput<TOutput>(TOutput outputToUse, SettingsCollection outputSettings)
            where TOutput : IResource, new()
        {
            return CreateOutput(outputToUse, SettingsCollection.ForOutput(), true);
        }

        /// <summary>
        /// Adds a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        /// <param name="outputSettings">The output settings to use for the command.</param>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public Command<TOutput> CreateOutput<TOutput>(TOutput outputToUse, SettingsCollection outputSettings, bool export)
            where TOutput : IResource, new()
        {
            return Command.OutputTo(this, outputToUse, outputSettings, export);
        }
        #endregion 

        /// <summary>
        /// Adds a new command using outputToUse as definition for the output file
        /// </summary>
        public CommandFactory Add(Command<IResource> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (Contains(command))
            {
                throw new ArgumentException("Command Factory already contains this command.", "command");
            }
            if (command.Parent.Id != Id)
            {
                throw new ArgumentException("Command was not created as a child of this factory.", "command");
            }

            CommandList.Add(command);
            return this;
        }

        /// <summary>
        /// Select the output resources for the current command factory 
        /// </summary>
        /// <returns></returns>
        public List<IResource> GetOutput()
        {
            return CommandList.Where(c => c.Output.IsExported)
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
                    return output.IsExported 
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
