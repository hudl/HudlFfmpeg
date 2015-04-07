namespace Hudl.FFmpeg.Common.DataTypes
{
    /// <summary>
    /// Describes a Red Green Blue intensity ratio that is used to intensify that colors
    /// </summary>
    public class DecimalScaleRgb
    {
        public DecimalScaleRgb()
        {
            Red = new DecimalScale();
            Green = new DecimalScale();
            Blue = new DecimalScale();
        }

        public DecimalScaleRgb(DecimalScale red, DecimalScale green, DecimalScale blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// property containing the double value of ratios to the Red color intensity
        /// </summary>
        public DecimalScale Red { get; set; } 

        /// <summary>
        /// property containing the double value of ratios to the Green color intensity
        /// </summary>
        public DecimalScale Green { get; set; } 

        /// <summary>
        /// property containing the double value of ratios to the Blue color intensity
        /// </summary>
        public DecimalScale Blue { get; set; } 
    }
}
