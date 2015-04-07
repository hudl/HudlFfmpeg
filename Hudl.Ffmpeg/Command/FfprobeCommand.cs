using System;
using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Metadata.FFprobe.Options.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    internal class FFprobeCommand : FFCommandBase
    {
        private FFprobeCommand(IContainer resource)
        {
            Resource = resource;
            Options = new List<IFFprobeOptions>();
        }

        public IContainer Resource { get; set; }

        public List<IFFprobeOptions> Options { get; set; }

        public static FFprobeCommand Create(IContainer resource)
        {
            return new FFprobeCommand(resource);    
        }

        public FFprobeCommand Register(IFFprobeOptions option)
        {
            if (option == null)
            {
                throw new ArgumentNullException("option");
            }

            Options.Add(option);

            return this;
        }

        public ICommandProcessor Execute()
        {
            return ExecuteWith<FFprobeProcessorReceiver>();
        }
    }
}
