namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// enumaration containing the known and extended codec libraries for the Hudl server
    /// </summary>
    /// <remarks></remarks>
    public enum AudioCodecTypes
    {
        /// <summary>
        /// indicates that the output aduio codec should be copied from the input 
        /// </summary>
        Copy,
        /// <summary>
        /// (experimental) indicates that the output audio should use AAC audio codec
        /// </summary>
        ExperimentalAac
    }
}
