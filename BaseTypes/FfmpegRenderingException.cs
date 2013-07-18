using System;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// exception that is thrown when Ffmpeg encounters a rendering exception.
    /// </summary>
    public class FfmpegRenderingException: Exception
    {
        public FfmpegRenderingException(Exception innerException)
            : base("Ffmpeg encouuntered an exception while attempting to render the file.", innerException)
        {
        }
    }
}
