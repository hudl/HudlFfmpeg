namespace Hudl.FFmpeg.Enums
{
    /// <summary>
    /// Video fade units of measurement, used in the application of a Fade filter.
    /// </summary>
    public enum VideoUnitType
    {
        /// <summary>
        /// indicates in a Video unit of measure is in seconds.
        /// </summary>
        Seconds = 0,
        /// <summary>
        /// indicates in a Video unit of measure is in frames.
        /// </summary>
        Frames = 1,
        /// <summary>
        /// indicates in a Video unit of measure is in timbase units.
        /// </summary>
        Timebase = 2,
    }
}
