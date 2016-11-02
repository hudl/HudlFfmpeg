namespace Hudl.FFmpeg.Enums
{
    /// <summary>
    /// enumaration containing the known and extended codec libraries for the Hudl server
    /// </summary>
    /// <remarks></remarks>
    public enum AudioCodecType
    {
        /// <summary>
        /// indicates that the output aduio codec should be copied from the input 
        /// </summary>
        Copy,
        /// <summary>
        /// (experimental) indicates that the output audio should use AAC audio codec
        /// </summary>
        ExperimentalAac, 
        /// <summary>
        /// Windows included AAC encoder library, this is included in the windows builds of ffmpeg. 
        /// </summary>
        Libvo_AacEnc,
        /// <summary>
        /// High effeciency AAC encoder library, this must be enabled during compilation of ffmpeg or it will not work.
        /// </summary>
        LibFdk_Aac
    }
}
