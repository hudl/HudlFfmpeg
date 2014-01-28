using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;
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
        public CommandFactory AddToOutput(Commandv2 command)
        {
            command.Output.Resource.Path = Configuration.OutputPath;
            return Add(command, true);
        }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddToResources(Commandv2 command)
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
        /// Select the output and temp resources for the current command factory 
        /// </summary>
        public List<IResource> GetAllOutput()
        {
            return CommandList.SelectMany(c =>
                {
                    var outputTempList = new List<IResource>();
                    outputTempList.Add(c.Output.Resource);
                    var prepTempList = c.CommandList.Select(pc => pc.Output.Resource).ToList();
                    if (prepTempList.Count > 0)
                    {
                        outputTempList.AddRange(prepTempList);
                    }
                    return outputTempList;
                }).ToList();
        }

        /// <summary>
        /// Returns a boolean indicating if the command already exists in the factory
        /// </summary>
        public bool Contains(Commandv2 command)
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
            where TProcessor : ICommandProcessor, new()
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
            where TProcessor : ICommandProcessor
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
        internal List<Commandv2> CommandList { get; set; }
        #endregion

        #region Utility
        private CommandFactory Add(Commandv2 command, bool export)
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
