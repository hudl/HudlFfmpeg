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

        protected Thread MonitoringThread;
        protected StringBuilder OutputBuilder;
        protected readonly Process ProcessToListenTo;
        protected volatile bool _stopped;
        protected ManualResetEvent _stopSignal;

        public string LastLineReceived { get; set; }

        public void Listen()
        {
            //workaround for a bug in the mono process when attempting to read async from console output events
            // - link http://mono.1490590.n4.nabble.com/System-Diagnostic-Process-and-event-handlers-td3246096.html
            if (ResourceManagement.IsMonoRuntime())
            {
                ListenAsThread();
            }
            else
            {
                ListenAsAsync();
            }
        }
        protected abstract void ListenAsAsync();
        protected abstract void ListenAsThread();

        public void Stop()
        {
            _stopped = true;
            _stopSignal.WaitOne(1000);
            if (ResourceManagement.IsMonoRuntime())
            {
                try
                {
                    if (MonitoringThread.ThreadState != System.Threading.ThreadState.Stopped)
                    {
                        MonitoringThread.Abort();
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Unable to abort the monitoring thread", e);
                }
            }
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
        protected void HandleDataReceivedAsThread(string data)
        {
            HandleDataReceived(data);
        }
       
        public override string ToString()
        {
            return OutputBuilder.ToString();
        }
    }
}
