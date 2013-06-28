using System;
using Hudl.Ffmpeg.Command.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class BatchCommandProcessorReciever :
        ICommandProcessor
    {
        public CommandProcessorStatus Status
        {
            get { throw new NotImplementedException(); }
        }

        public bool Open()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool Send(string command)
        {
            throw new NotImplementedException();
        }
    }
}
