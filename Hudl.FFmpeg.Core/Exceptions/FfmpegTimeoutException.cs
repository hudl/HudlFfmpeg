using System;

namespace Hudl.FFmpeg.Exceptions
{
    /// <summary>
    /// exception that is thrown when FFmpeg commands have passed the timeout period
    /// </summary>
    public class FFmpegTimeoutException : Exception
    {
        public FFmpegTimeoutException(string arguments)
            : base("FFmpeg timed out during processing")
        {
            base.Data["Arguments"] = arguments;
            base.Data["ErrorOutput"] = errorOutput;

            Arguments = arguments;
            ErrorOutput = errorOutput; 
        }

        public string Arguments { get; private set; }

        public string ErrorOutput { get; private set; }
    }
}
