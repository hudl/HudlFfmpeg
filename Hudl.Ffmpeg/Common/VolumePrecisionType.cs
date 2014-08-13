namespace Hudl.FFmpeg.Common
{
    public enum VolumePrecisionType
    {
        /// <summary>
        /// 32-bit floating-point; this limits input sample format to FLT. (default)
        /// </summary>
        Float,
        /// <summary>
        /// 8-bit fixed-point; this limits input sample format to U8, S16, and S32.
        /// </summary>
        Fixed,
        /// <summary>
        /// 64-bit floating-point; this limits input sample format to DBL.
        /// </summary>
        Double
    }
}
