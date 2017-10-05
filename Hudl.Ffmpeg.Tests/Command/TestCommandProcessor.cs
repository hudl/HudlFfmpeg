using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using System;
using System.Threading;

namespace Hudl.FFmpeg.Tests.Command
{
    public class TestCommandProcessor : ICommandProcessor
    {
        public bool OpenFired { get; set; }
        public bool CloseFired { get; set; }
        public bool SendFired { get; set; }

        public CommandProcessorStatus Status { get; protected set; }

        public string StdOut { get; protected set; }

        public string Command { get; protected set; }

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
            return Send(command, default(CancellationToken));
        }

        public bool Send(string command, CancellationToken token = default(CancellationToken))
        {
            Command = command;
            SendFired = true;
            return true;
        }
    }
}
