using System;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// exception that is thrown when Ffmpeg returns an exit code other than 0.
    /// </summary>
    public class FfmpegProcessingException: Exception
    {
        public FfmpegProcessingException(int exitCode, string errorOutput)
            : base(string.Format("Ffmpeg failed processing with an exit code of {0}",
                   exitCode))
        {
            base.Data["ExitCode"] = exitCode;
            base.Data["ErrorOutput"] = errorOutput;

            ExitCode = exitCode;
            ErrorOutput = errorOutput; 
        }

        public int ExitCode { get; private set; }

        public string ErrorOutput { get; private set; }
    }
}
