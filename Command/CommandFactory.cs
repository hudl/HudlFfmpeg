using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// Command Factory is a management list of all the commands to be run in an Ffmpeg job.
    /// </summary>
    public class CommandFactory
    {
        public CommandFactory(CommandConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            Id = Guid.NewGuid().ToString();
            Configuration = configuration;
            CommandList = new List<Command<IResource>>();
        }

        /// <summary>
        /// Returns the count of commands that are in the factory, excluding prep commands
        /// </summary>
        public int Count { get { return CommandList.Count; } }

        /// <summary>
        /// Returns the path location for the command factory to store the output files.
        /// </summary>
        public CommandConfiguration Configuration { get; private set; }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddToOutput(Command<IResource> command)
        {
            command.Output.Resource.Path = Configuration.OutputPath;
            return Add(command, true);
        }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddToResources(Command<IResource> command)
        {
            command.Output.Resource.Path = Configuration.TempPath;
            return Add(command, false);
        }

        /// <summary>
        /// Select the output resources for the current command factory 
        /// </summary>
        public List<IResource> GetOutput()
        {
            return CommandList.Where(c => c.Output.IsExported)
                              .Select(c => c.Output.Resource).ToList();
        }

        /// <summary>
        /// Returns a boolean indicating if the command already exists in the factory
        /// </summary>
        public bool Contains<TOutput>(Command<TOutput> command)
            where TOutput : IResource
        {
            return CommandList.Any(c => c.Id == command.Id);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IResource> Render()
        {
            return RenderWith<WinCmdProcessorReciever>();
        }

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open(Configuration))
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
                        ? output.GetOutput() 
                        : null;
                }).ToList();
        }

        #region Command Helper Visibility
        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>()
            where TOutput : class, IResource, new()
        {
            var temporaryResource = new TOutput();
            return CreateOutput<TOutput>(SettingsCollection.ForOutput(), temporaryResource.Name);
        }

        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>(string fileName)
            where TOutput : class, IResource, new()
        {
            return CreateOutput<TOutput>(SettingsCollection.ForOutput(), fileName);
        }

        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>(SettingsCollection settings, string fileName)
            where TOutput : class, IResource, new()
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (string.IsNullOrWhiteSpace("fileName"))
            {
                throw new ArgumentException("Output file name cannot be empty", "fileName");
            }

            var newResource = Resource.Create<TOutput>(Configuration.OutputPath, fileName);
            return Command.OutputTo(this, newResource, settings);
        }
        #endregion 

        #region Internals
        internal string Id { get; set; }
        internal List<Command<IResource>> CommandList { get; set; }
        #endregion

        #region Utility
        private CommandFactory Add(Command<IResource> command, bool export)
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

            command.Output.IsExported = export;
            CommandList.Add(command);
            return this;
        }
        #endregion 
    }
}
