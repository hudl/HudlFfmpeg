namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// enumaration containing the known bit rate configurations
    /// </summary>
    public enum AudioBitRateType
    {
        /// <summary>
        /// indicates Mp3 'Cd' quality bit rate of 256kbs
        /// </summary>
        Mp3Cd = 256,
        /// <summary>
        /// indicates Mp3 'Fm radio' quality bit rate of 192kbs
        /// </summary>
        Mp3Fm = 192,
        /// <summary>
        /// indicates Mp3 'Good' quality bit rate of 160kbs
        /// </summary>
        Mp3Good = 160,
        /// <summary>
        /// indicates Mp3 'Fair' quality bit rate of 128kbs
        /// </summary>
        Mp3Fair = 128,
        /// <summary>
        /// indicates Mp3 'Poor' quality bit rate of 96kbs
        /// </summary>
        Mp3Poor = 96,
        /// <summary>
        /// indicates Mp3 'Lousy' quality bit rate of 64kbs
        /// </summary>
        Mp3Lousy = 64,
        /// <summary>
        /// indicates Mp3 'Terrible' quality bit rate of 32kbs
        /// </summary>
        Mp3Terrible = 32
    }
}
