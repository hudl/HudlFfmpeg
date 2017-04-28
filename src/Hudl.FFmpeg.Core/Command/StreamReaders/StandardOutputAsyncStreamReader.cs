using System.Diagnostics;

namespace Hudl.FFmpeg.Command.StreamReaders
{
    public class StandardOutputAsyncStreamReader : BaseStandardStreamReader
    {
        private StandardOutputAsyncStreamReader(Process processToListenTo)
            : base(processToListenTo)
        {
            ProcessToListenTo.StartInfo.RedirectStandardOutput = true;
            ProcessToListenTo.ErrorDataReceived += HandleDataReceivedAsAsync;
        }

        public static StandardOutputAsyncStreamReader AttachReader(Process processToListenTo)
        {
            return new StandardOutputAsyncStreamReader(processToListenTo);
        }

        protected override void ListenAsAsync()
        {
            ProcessToListenTo.BeginErrorReadLine();
        }
    }
}
