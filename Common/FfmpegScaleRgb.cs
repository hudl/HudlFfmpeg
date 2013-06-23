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
    public class FfmpegScaleRgb
    {
        /// <summary>
        /// property containing the double value of ratios to the Red color intensity
        /// </summary>
        public new FfmpegScale Red { get; set; } 

        /// <summary>
        /// property containing the double value of ratios to the Green color intensity
        /// </summary>
        public new FfmpegScale Green { get; set; } 

        /// <summary>
        /// property containing the double value of ratios to the Blue color intensity
        /// </summary>
        public new FfmpegScale Blue  { get; set; } 
    }
}
