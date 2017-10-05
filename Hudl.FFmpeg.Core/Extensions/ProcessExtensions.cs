using System;
using System.Diagnostics;
using System.Threading;

namespace Hudl.FFmpeg.Extensions
{
    public static class ProcessExtensions
    {
        public static bool WaitForProcessStart(this Process process)
        {
            return process.WaitForProcessStart(null);
        }
        public static bool WaitForProcessStart(this Process process, int? timeoutMilliseconds)
        {
            var processTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds ?? 10000);
            return process.WaitForProcessStart(processTimeout);
        }
        public static bool WaitForProcessStart(this Process process, TimeSpan processTimeout)
        {
            var processStopwatch = Stopwatch.StartNew();
            var isProcessRunning = false;

            while (processStopwatch.ElapsedMilliseconds <= processTimeout.TotalMilliseconds && !isProcessRunning)
            {
                //give a little bit of breathing room here....
                Thread.Sleep(20.Milliseconds());

                try
                {
                    isProcessRunning = (process != null && process.HasExited && process.Id != 0);
                }
                catch
                {
                    isProcessRunning = false;
                }
            }

            return isProcessRunning;
        }

        public static bool WaitForProcessStop(this Process process)
        {
            return process.WaitForProcessStop(default(CancellationToken));
        }
        public static bool WaitForProcessStop(this Process process, CancellationToken token = default(CancellationToken))
        {
            var processStopwatch = Stopwatch.StartNew();

            while (!process.HasExited)
            {
                Thread.Sleep(1.Seconds());

                token.ThrowIfCancellationRequested(); 
            }

            return true;
        }
    }
}
