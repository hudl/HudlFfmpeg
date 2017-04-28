using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Resources.Interfaces;
using log4net; 

namespace Hudl.FFmpeg.Command
{
    /// <summary>
    /// Command Factory is a management list of all the commands to be run in an FFmpeg job.
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CommandFactory));

        private CommandFactory()
        {
            Id = Guid.NewGuid().ToString();
            CommandList = new List<ICommand>();
        }

        public static CommandFactory Create()
        {
            if (ResourceManagement.CommandConfiguration == null)
            {
                throw new InvalidOperationException("A command factory cannot be created without a global configuration set Hudl.FFmpeg.ResourceManagement.CommandConfiguration");   
            }

            return new CommandFactory();
        }

        public string Id { get; set; }

        public List<ICommand> CommandList { get; set; }

        /// <summary>
        /// Returns the count of commands that are in the factory, excluding prep commands
        /// </summary>
        public int Count { get { return CommandList.Count; } }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddCommandAsOutput(FFmpegCommand command)
        {
            command.Objects.Outputs.ForEach(output => output.Resource.Path = ResourceManagement.CommandConfiguration.OutputPath);

            return Add(command, true);
        }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddCommandAsResource(FFmpegCommand command)
        {
            command.Objects.Outputs.ForEach(output => output.Resource.Path = ResourceManagement.CommandConfiguration.TempPath);

            return Add(command, false);
        }

        /// <summary>
        /// Select the all IResources marked as an exported output.
        /// </summary>
        public List<IContainer> GetOutputs()
        {
            return CommandList.OfType<FFmpegCommand>()
                .Where(c => c.Outputs.Any(cr => cr.IsExported))
                .SelectMany(c => c.Outputs.Where(cr => cr.IsExported).Select(cr => cr.Resource))
                .ToList();
        }

        /// <summary>
        /// Select the all IResources not marked as an exported output.
        /// </summary>
        public List<IContainer> GetResources()
        {
            return CommandList.OfType<FFmpegCommand>()
                .Where(c => c.Outputs.Any(cr => !cr.IsExported))
                .SelectMany(c => c.Outputs.Where(cr => !cr.IsExported).Select(cr => cr.Resource))
                .ToList();
        }

        /// <summary>
        /// Select the output and temp resources for the current command factory 
        /// </summary>
        public List<IContainer> GetAllOutput()
        {
            return CommandList.OfType<FFmpegCommand>()
                .SelectMany(c => c.Outputs.Select(cr => cr.Resource))
                .ToList();
        }

        /// <summary>
        /// Returns a boolean indicating if the command already exists in the factory
        /// </summary>
        public bool Contains(FFmpegCommand command)
        {
            return CommandList.OfType<FFmpegCommand>()
                .Any(c => c.Id == command.Id);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IContainer> Render()
        {
            return RenderWith<FFmpegCommandProcessor, FFmpegCommandBuilder>(null);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IContainer> Render(int timeoutMilliseconds)
        {
            return RenderWith<FFmpegCommandProcessor, FFmpegCommandBuilder>(timeoutMilliseconds);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IContainer> Render(TimeSpan timeout)
        {
            return RenderWith<FFmpegCommandProcessor, FFmpegCommandBuilder>((int)timeout.TotalMilliseconds);
        }

        private CommandFactory Add(FFmpegCommand command, bool export)
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

        internal List<IContainer> RenderWith<TProcessor, TBuilder>(int? timeoutMilliseconds)
            where TProcessor : class, ICommandProcessor, new()
            where TBuilder : class, ICommandBuilder, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            var returnType = RenderWith<TProcessor, TBuilder>(commandProcessor, timeoutMilliseconds);

            if (!commandProcessor.Close())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        internal List<IContainer> RenderWith<TProcessor, TBuilder>(TProcessor processor, int? timeoutMilliseconds)
            where TProcessor : class, ICommandProcessor, new()
            where TBuilder : class, ICommandBuilder, new()
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            var outputList = GetOutputs();

            Log.InfoFormat("Rendering command factory Outputs={0} Commands={1}",
                outputList.Count,
                CommandList.Count);

            CommandList.OfType<FFmpegCommand>()
                .ToList()
                .ForEach(command => command.ExecuteWith<TProcessor, TBuilder>(processor, timeoutMilliseconds)); 

            return outputList;
        }
        #endregion
    }
}
