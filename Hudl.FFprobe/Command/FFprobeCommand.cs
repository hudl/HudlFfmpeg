using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFprobe.Options.BaseTypes;

namespace Hudl.FFprobe.Command
{
    internal class FFprobeCommand : FFcommandBase
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
            return ExecuteWith<FFprobeCommandProcessor, FFprobeCommandBuilder>();
        }
    }
}
