namespace Hudl.Ffmpeg.Common
{
    public enum VolumeReplayGainType
    {
        /// <summary>
        /// remove ReplayGain side data, ignoring its contents (the default).
        /// </summary>
        Drop,
        /// <summary>
        /// ignore ReplayGain side data, but leave it in the frame.
        /// </summary>
        Ignore,
        /// <summary>
        /// prefer the track gain, if present.
        /// </summary>
        Track,
        /// <summary>
        /// prefer the album gain, if present.
        /// </summary>
        Album
    }
}
