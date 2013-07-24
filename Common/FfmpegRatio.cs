using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Describes an aspect ratio type used to define dynamic and sample aspect ratios.
    /// </summary>
    public class FfmpegRatio
    {
        private readonly int _numerator;
        private readonly int _denominator;

        public FfmpegRatio(int numerator, int denominator) 
        {
            if (numerator <= 0)
            {
                throw new ArgumentException("The FfmpegRatio numerator must be greater than zero.", "numerator");
            }
            if (denominator <= 0)
            {
                throw new ArgumentException("The FfmpegRatio denominator must be greater than zero.", "denominator");
            }

            _numerator = numerator;
            _denominator = denominator;
        }

        public override string ToString()
        {
            return string.Concat(_numerator, "/", _denominator); 
        }

        public string ToRatio()
        {
            return string.Concat(_numerator, ":", _denominator); 
        }
    }
}
