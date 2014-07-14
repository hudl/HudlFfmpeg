using System;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Tests.Command
{
    public class TestCommandProcessor : ICommandProcessor
    {
        public bool OpenFired { get; set; }
        public bool CloseFired { get; set; }
        public bool SendFired { get; set; }

        public CommandProcessorStatus Status { get; protected set; }

        public string StdOut { get; protected set; }

        public Exception Error { get; protected set; }

        public bool Open()
        {
            OpenFired = true;
            return true; 
        }

        public bool Close()
        {
            CloseFired = true;
            return true;
        }

        public bool Send(string command)
        {
            SendFired = true;
            return true;
        }
    }
}
