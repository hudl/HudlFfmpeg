using System;

namespace Hudl.FFmpeg.Exceptions
{
    /// <summary>
    /// invalid applies to exception, thrown when Applier does not apply to AppliedTo
    /// </summary>
    public class StreamNotFoundException: Exception
    {
        public StreamNotFoundException()
            : base("No streams could be found.")
        {
        }

        public StreamNotFoundException(Type streamType)
            : base(string.Format("Stream type of '{0}' could not be found.",
                   streamType.Name))
        {
        }
    }
}
