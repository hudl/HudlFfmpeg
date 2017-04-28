using System;

namespace Hudl.FFmpeg.Exceptions
{
    /// <summary>
    /// exception that is thrown when Hudl.FFmpeg encounters a rendering exception.
    /// </summary>
    public class FFmpegRenderingException: Exception
    {
        public FFmpegRenderingException(Exception innerException)
            : base("FFmpeg encouuntered an exception while attempting to render the file.", innerException)
        {
        }
    }
}
