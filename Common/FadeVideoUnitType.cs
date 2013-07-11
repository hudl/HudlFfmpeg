namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Video fade units of measurement, used in the application of a Fade filter.
    /// </summary>
    public enum FadeVideoUnitType
    {
        /// <summary>
        /// indicates in a Video Fade filter the unit of measure is in seconds.
        /// </summary>
        Seconds = 0,
        /// <summary>
        /// indicates in a Video Fade filter the unit of measure is in frames.
        /// </summary>
        Frames = 1 
    }
}
