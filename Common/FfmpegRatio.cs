using System;
using System.Globalization;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Describes an aspect ratio type used to define dynamic and sample aspect ratios.
    /// </summary>
    public class FfmpegRatio
    {
        private readonly bool _decimalSet; 
        private readonly int _numerator;
        private readonly int _denominator;
        private readonly decimal _aspectRatio; 

        public FfmpegRatio(decimal aspectRatio)
        {
            if (aspectRatio <= 0)
            {
                throw new ArgumentException("The FfmpegRatio aspectRatio must be greater than zero.", "aspectRatio");
            }

            _decimalSet = true; 
            _aspectRatio = aspectRatio; 
        }
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

            _decimalSet = false;
            _numerator = numerator;
            _denominator = denominator;
        }

        public override string ToString()
        {
            return _decimalSet 
                ? _aspectRatio.ToString(CultureInfo.InvariantCulture) 
                : string.Concat(_numerator, "/", _denominator); 
        }

        public string ToRatio()
        {
            return _decimalSet
                ? _aspectRatio.ToString(CultureInfo.InvariantCulture)
                : string.Concat(_numerator, ":", _denominator); 
        }

        public static FfmpegRatio Create(decimal aspectRatio)
        {
            return new FfmpegRatio(aspectRatio);
        }

        public static FfmpegRatio Create(int numerator, int denominator)
        {
            return new FfmpegRatio(numerator, denominator);
        }
    }
}
