using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    internal class FfprobeCommand
    {
        private FfprobeCommand(IResource resource)
        {
            Resource = resource;
            Serializers = new List<IFfprobeSerializer>();
        }

        public IResource Resource { get; set; }

        public List<IFfprobeSerializer> Serializers { get; set; }

        public static FfprobeCommand Create(IResource resource)
        {
            return new FfprobeCommand(resource);    
        }

        public FfprobeCommand Register(IFfprobeSerializer serializer)
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
            return ExecuteWith<FfprobeProcessorReceiver>();
        }

        public ICommandProcessor ExecuteWith<TProcessor>()
            where TProcessor : class, ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open())
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            var returnType = ExecuteWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
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
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return commandProcessor;
        }
    }
}
