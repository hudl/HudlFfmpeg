using System;
using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    internal class FFprobeCommand
    {
        private FFprobeCommand(IContainer resource)
        {
            Resource = resource;
            Serializers = new List<IFFprobeSerializer>();
        }

        public IContainer Resource { get; set; }

        public List<IFFprobeSerializer> Serializers { get; set; }

        public static FFprobeCommand Create(IContainer resource)
        {
            return new FFprobeCommand(resource);    
        }

        public FFprobeCommand Register(IFFprobeSerializer serializer)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            Serializers.Add(serializer);

            return this;
        }

        public ICommandProcessor Execute()
        {
            return ExecuteWith<FFprobeProcessorReceiver>();
        }

        public ICommandProcessor ExecuteWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            var returnType = ExecuteWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        public ICommandProcessor ExecuteWith<TProcessor>(TProcessor commandProcessor)
            where TProcessor : class, ICommandProcessor
        {
            if (commandProcessor == null)
            {
                throw new ArgumentNullException("commandProcessor");
            }

            var commandBuilder = new CommandBuilder();
            commandBuilder.WriteCommand(this);

            if (!commandProcessor.Send(commandBuilder.ToString()))
            {
                throw new FFmpegRenderingException(commandProcessor.Error);
            }

            return commandProcessor;
        }
    }
}
