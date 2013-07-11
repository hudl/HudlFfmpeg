namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// enumaration containing the know and extended codec libraries for the Hudl server
    /// </summary>
    /// <remarks></remarks>
    public enum VideoCodecType
    {
        /// <summary>
        /// indicates that the output video codec should be copied from the input 
        /// </summary>
        Copy,
        /// <summary>
        /// indicates that the output video should use the h.264 codec
        /// </summary>
        Libx264
    }
}
