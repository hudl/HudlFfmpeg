using System.Diagnostics;
using System.Threading;

namespace Hudl.FFmpeg.Command.StreamReaders
{
    public class StandardErrorAsyncStreamReader : BaseStandardStreamReader
    {
        private StandardErrorAsyncStreamReader(Process processToListenTo)
            : base(processToListenTo)
        {
            ProcessToListenTo.StartInfo.RedirectStandardError = true;
            ProcessToListenTo.ErrorDataReceived += HandleDataReceivedAsAsync;
        }

        public static StandardErrorAsyncStreamReader AttachReader(Process processToListenTo)
        {
            return new StandardErrorAsyncStreamReader(processToListenTo);
        }

        protected override void ListenAsAsync()
        {
            ProcessToListenTo.BeginErrorReadLine();
        }
    }

}
