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
            return process.WaitForProcessStop(null);
        }
        public static bool WaitForProcessStop(this Process process, int? timeoutMilliseconds)
        {
            var processTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds ?? 0);
            return process.WaitForProcessStop(processTimeout);
        }
        public static bool WaitForProcessStop(this Process process, TimeSpan processTimeout)
        {
            var processStopwatch = Stopwatch.StartNew();

            while (!process.HasExited)
            {
                Thread.Sleep(1.Seconds());

                if (processTimeout.TotalMilliseconds > 0 && processStopwatch.ElapsedMilliseconds > processTimeout.TotalMilliseconds)
                {
                    process.Kill();

                    process.WaitForExit((int)5.Seconds().TotalMilliseconds);

                    return false;
                }
            }

            return true;
        }
    }
}
