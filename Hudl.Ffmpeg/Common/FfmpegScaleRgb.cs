namespace Hudl.FFmpeg.Common
{
    /// <summary>
    /// Describes a Red Green Blue intensity ratio that is used to intensify that colors
    /// </summary>
    public class FFmpegScaleRgb
    {
        public FFmpegScaleRgb()
        {
            Red = new FFmpegScale();
            Green = new FFmpegScale();
            Blue = new FFmpegScale();
        }

        public FFmpegScaleRgb(FFmpegScale red, FFmpegScale green, FFmpegScale blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// property containing the double value of ratios to the Red color intensity
        /// </summary>
        public FFmpegScale Red { get; set; } 

        /// <summary>
        /// property containing the double value of ratios to the Green color intensity
        /// </summary>
        public FFmpegScale Green { get; set; } 

        /// <summary>
        /// property containing the double value of ratios to the Blue color intensity
        /// </summary>
        public FFmpegScale Blue  { get; set; } 
    }
}
