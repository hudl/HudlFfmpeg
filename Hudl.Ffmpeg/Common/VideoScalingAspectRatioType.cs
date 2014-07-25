namespace Hudl.FFmpeg.Common
{
    public enum VideoScalingAspectRatioType
    {
        /// <summary>
        /// scale the video as specified and disable this feature.
        /// </summary>
        Disable = 0, 
        /// <summary>
        /// the output video dimensions will automatically be decreased if needed.
        /// </summary>
        Decrease = 1, 
        /// <summary>
        /// the output video dimensions will automatically be increased if needed.
        /// </summary>
        Increase = 2
    }
}
