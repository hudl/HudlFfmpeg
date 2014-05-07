using System;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// exception that is thrown when Ffmpeg encounters a processing exception.
    /// </summary>
    public class FfmpegProcessingException: Exception
    {
        public FfmpegProcessingException(int exitCode, string errorOutput)
            : base(string.Format("Ffmpeg failed processing with an exit code of {0}",
                   exitCode))
        {
            base.Data["ExitCode"] = exitCode;
            base.Data["ErrorOutput"] = errorOutput;
        }
    }
}
