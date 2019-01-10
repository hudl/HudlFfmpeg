namespace Hudl.FFmpeg.Enums
{
    /// <summary>
    /// Overlay format commands for an Overlay filter type
    /// </summary>
    public enum OverlayVideoFormatType
    {
        /// <summary>
        /// forces a YUV420 pixel output format
        /// </summary>
        Yuv420 = 0,
        /// <summary>
        /// forces a YUV444 pixel output format
        /// </summary>
        Yuv444 = 1,
        /// <summary>
        /// forces an RGB pixel output format
        /// </summary>
        Rgb = 2
        /// <summary>
        /// forces a YUV420P pixel output format
        /// </summary>
        Yuv420P = 3
    }

}
