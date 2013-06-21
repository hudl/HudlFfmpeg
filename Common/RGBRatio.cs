using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Describes a Red Green Blue intensity ratio that is used to intensify that colors
    /// </summary>
    public class RGBRatio
    {
        /// <summary>
        /// property containing the double value of ratios to the Red color intensity
        /// </summary>
        public double Red 
        {
            get
            {
                return _red;
            }
            set
            {
                if (value > 1 || value < -1)
                   throw new ArgumentException("Value must be between -1 and 1", "value");
                _red = value;
            }
        }
        private double _red = 0D;

        /// <summary>
        /// property containing the double value of ratios to the Green color intensity
        /// </summary>
        public double Green 
        {
            get
            {
                return _green;
            }
            set
            {
                if (value > 1 || value < -1)
                   throw new ArgumentException("Value must be between -1 and 1", "value");
                _green = value;
            }
        }
        private double _green = 0D;

        /// <summary>
        /// property containing the double value of ratios to the Blue color intensity
        /// </summary>
        public double Blue 
        {
            get
            {
                return _blue;
            }
            set
            {
                if (value > 1 || value < -1)
                   throw new ArgumentException("Value must be between -1 and 1", "value");
                _blue = value;
            }
        }
        private double _blue = 0D;
    }
}
