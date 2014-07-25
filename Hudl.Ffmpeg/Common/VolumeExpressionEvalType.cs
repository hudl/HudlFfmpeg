namespace Hudl.FFmpeg.Common
{
    public enum VolumeExpressionEvalType
    {
        /// <summary>
        /// only evaluate expression once during the filter initialization, or when the ‘volume’ command is sent. (default)
        /// </summary>
        Once,
        /// <summary>
        /// evaluate expression for each incoming frame
        /// </summary>
        Frame
    }
}
