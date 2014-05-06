using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using log4net; 

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// Command Factory is a management list of all the commands to be run in an Ffmpeg job.
    /// </summary>
    public class CommandFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CommandFactory).Name);

        public CommandFactory(CommandConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            Id = Guid.NewGuid().ToString();
            Configuration = configuration;
            CommandList = new List<FfmpegCommand>();
        }

        public static CommandFactory Create(CommandConfiguration configuration)
        {
            return new CommandFactory(configuration);
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
        public CommandFactory AddToOutput(FfmpegCommand command)
        {
            command.Objects.Outputs.ForEach(output => output.Resource.Path = Configuration.OutputPath);
            return Add(command, true);
        }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddToResources(FfmpegCommand command)
        {
            command.Objects.Outputs.ForEach(output => output.Resource.Path = Configuration.TempPath);
            return Add(command, false);
        }

        /// <summary>
        /// Select the output resources for the current command factory 
        /// </summary>
        public List<IResource> GetOutput()
        {
            return CommandList.Where(c => c.Outputs.Any(cr => cr.IsExported))
                              .SelectMany(c =>
                                  {
                                      var outputTempList = new List<IResource>();
                                      outputTempList.AddRange(c.Outputs.Where(cr => cr.IsExported).Select(cr => cr.Resource));
                                      return outputTempList;
                                  })
                              .ToList();
        }

        /// <summary>
        /// Select the output and temp resources for the current command factory 
        /// </summary>
        public List<IResource> GetAllOutput()
        {
            return CommandList.SelectMany(c => c.Outputs.Select(cr => cr.Resource))
                              .ToList();
        }

        /// <summary>
        /// Returns a boolean indicating if the command already exists in the factory
        /// </summary>
        public bool Contains(FfmpegCommand command)
        {
            return CommandList.Any(c => c.Id == command.Id);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IResource> Render()
        {
            return RenderWith<CmdProcessorReciever>();
        }

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open(Configuration))
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        /// <summary>
        /// Renders the command stream with an existing command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : class, ICommandProcessor
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            var outputList = GetOutput();

            Log.InfoFormat("Rendering command factory Outputs={0} Commands={1}", 
                outputList.Count,
                CommandList.Count);

            CommandList.ForEach(command => command.RenderWith(processor));

            return outputList;
        }

        #region Internals
        internal string Id { get; set; }
        internal List<FfmpegCommand> CommandList { get; set; }
        #endregion

        #region Utility
        private CommandFactory Add(FfmpegCommand command, bool export)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (Contains(command))
            {
                throw new ArgumentException("Command Factory already contains this command.", "command");
            }
            if (command.Owner.Id != Id)
            {
                throw new ArgumentException("Command was not created as a child of this factory.", "command");
            }

            command.Objects.Outputs.ForEach(output => output.IsExported = export);

            CommandList.Add(command);

            return this;
        }
        #endregion 
    }
}
