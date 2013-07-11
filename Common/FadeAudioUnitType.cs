namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Audio fade units of measurement, used in the application of an AFade filter.
    /// </summary>
    public enum FadeAudioUnitType
    {
        // ReSharper disable UnusedMember.Local
        /// <summary>
        /// indicates the unit of measure to apply the fade is in seconds
        /// </summary>
        Seconds = 0,
        /// <summary>
        /// indicates the unit of measure to apply the fade is in samples
        /// </summary>
        Sample = 1
        // ReSharper restore UnusedMember.Local
    }
}
