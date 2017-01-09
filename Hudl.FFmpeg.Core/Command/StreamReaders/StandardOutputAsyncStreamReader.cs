using System.Diagnostics;
using System.Threading;

namespace Hudl.FFmpeg.Command.StreamReaders
{
    public class StandardOutputAsyncStreamReader : BaseStandardStreamReader
    {
        private StandardOutputAsyncStreamReader(Process processToListenTo)
            : base(processToListenTo)
        {
            ProcessToListenTo.StartInfo.RedirectStandardOutput = true;
            if (!ResourceManagement.IsMonoRuntime())
            {
                ProcessToListenTo.ErrorDataReceived += HandleDataReceivedAsAsync;
            }
        }

        public static StandardOutputAsyncStreamReader AttachReader(Process processToListenTo)
        {
            return new StandardOutputAsyncStreamReader(processToListenTo);
        }

        protected override void ListenAsAsync()
        {
            ProcessToListenTo.BeginErrorReadLine();
        }

        protected override void ListenAsThread()
        {
            MonitoringThread = new Thread(AsyncStdErrorMonitor);
            MonitoringThread.Start();
        }

        private void AsyncStdErrorMonitor()
        {
            try
            {
                do
                {
                    string line = ProcessToListenTo.StandardError.ReadLine();
                    if (line != null)
                    {
                        HandleDataReceivedAsThread(line);
                    }
                }
                while (!_stopped && !ProcessToListenTo.HasExited);
                _stopSignal.Set();
            }
            catch
            { }
        }
    }

}
