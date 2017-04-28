using Hudl.FFmpeg.Logging;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Hudl.FFmpeg.Command.StreamReaders
{
    public abstract class BaseStandardStreamReader
    {
        private static readonly LogUtility Log = LogUtility.GetLogger(typeof(BaseStandardStreamReader));
        private const int OutputBuilderLimit = 10000;

        protected BaseStandardStreamReader(Process processToListenTo)
        {
            OutputBuilder = new StringBuilder();
            ProcessToListenTo = processToListenTo;
            _stopSignal = new ManualResetEvent(false);
        }

        protected StringBuilder OutputBuilder;
        protected readonly Process ProcessToListenTo;
        protected volatile bool _stopped;
        protected ManualResetEvent _stopSignal;

        public string LastLineReceived { get; set; }

        public void Listen()
        {
            ListenAsAsync();
        }
        protected abstract void ListenAsAsync();

        public void Stop()
        {
            _stopped = true;
            _stopSignal.WaitOne(1000);
        }

        private void HandleDataReceived(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                LastLineReceived = data;
            }

            //there is no specific reason behind this number other than, it seems like a pretty good number baseline. 
            //ultimately ffmpeg can blow up the standard output/error stream with logs. we dont want to create
            //an OOM exception. so we will trash and reset.
            if (OutputBuilder.Length > OutputBuilderLimit)
            {
                var newBuilder = new StringBuilder(OutputBuilderLimit);
                newBuilder.Append(OutputBuilder.ToString(), OutputBuilderLimit / 2, OutputBuilderLimit / 2);
                OutputBuilder = newBuilder;
            }

            OutputBuilder.AppendLine(data);
        }
        protected void HandleDataReceivedAsAsync(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            HandleDataReceived(dataReceivedEventArgs.Data);
        }
       
        public override string ToString()
        {
            return OutputBuilder.ToString();
        }
    }
}
