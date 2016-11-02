namespace Hudl.FFmpeg.Enums
{
    public enum VideoScalingRangeType
    {
        /// <summary>
        /// choose automatically (default).
        /// </summary>
        Auto = 0,
        /// <summary>
        /// set full range (0-255 in case of 8-bit luma).
        /// </summary>
        Jpeg_Full_Pc = 1,
        /// <summary>
        /// set "MPEG" range (16-235 in case of 8-bit luma).
        /// </summary>
        Mpeg_Tv = 2
    }
}
