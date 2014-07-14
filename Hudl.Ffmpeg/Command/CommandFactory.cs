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

        private CommandFactory()
        {
            Id = Guid.NewGuid().ToString();
            CommandList = new List<FfmpegCommand>();
        }

        public static CommandFactory Create()
        {
            if (ResourceManagement.CommandConfiguration == null)
            {
                throw new InvalidOperationException("A command factory cannot be created without a global configuration set Hudl.Ffmpeg.ResourceManagement.CommandConfiguration");   
            }

            return new CommandFactory();
        }

        /// <summary>
        /// Returns the count of commands that are in the factory, excluding prep commands
        /// </summary>
        public int Count { get { return CommandList.Count; } }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddCommandAsOutput(FfmpegCommand command)
        {
            command.Objects.Outputs.ForEach(output => output.Resource.Path = ResourceManagement.CommandConfiguration.OutputPath);

            return Add(command, true);
        }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddCommandAsResource(FfmpegCommand command)
        {
            command.Objects.Outputs.ForEach(output => output.Resource.Path = ResourceManagement.CommandConfiguration.TempPath);

            return Add(command, false);
        }

        /// <summary>
        /// Select the all IResources marked as an exported output.
        /// </summary>
        public List<IResource> GetOutputs()
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
        /// Select the all IResources not marked as an exported output.
        /// </summary>
        public List<IResource> GetResources()
        {
            return CommandList.Where(c => c.Outputs.Any(cr => !cr.IsExported))
                              .SelectMany(c =>
                              {
                                  var outputTempList = new List<IResource>();
                                  outputTempList.AddRange(c.Outputs.Where(cr => !cr.IsExported).Select(cr => cr.Resource));
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
            return RenderWith<FfmpegProcessorReciever>();
        }

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

        #region Internals
        internal string Id { get; set; }

        internal List<FfmpegCommand> CommandList { get; set; }

        internal List<IResource> RenderWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
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

        internal List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : class, ICommandProcessor
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            var outputList = GetOutputs();

            Log.InfoFormat("Rendering command factory Outputs={0} Commands={1}",
                outputList.Count,
                CommandList.Count);

            CommandList.ForEach(command => command.RenderWith(processor));

            return outputList;
        }
        #endregion
    }
}
