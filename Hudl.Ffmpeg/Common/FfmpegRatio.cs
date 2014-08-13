using System;
using System.Globalization;

namespace Hudl.FFmpeg.Common
{
    /// <summary>
    /// Describes an aspect ratio type used to define dynamic and sample aspect ratios.
    /// </summary>
    public class FFmpegRatio
    {
        private readonly bool _decimalSet; 
        private readonly int _numerator;
        private readonly int _denominator;
        private readonly decimal _aspectRatio; 

        public FFmpegRatio(decimal aspectRatio)
        {
            if (aspectRatio <= 0)
            {
                throw new ArgumentException("The FFmpegRatio aspectRatio must be greater than zero.", "aspectRatio");
            }

            _decimalSet = true; 
            _aspectRatio = aspectRatio; 
        }
        public FFmpegRatio(int numerator, int denominator) 
        {
            if (numerator <= 0)
            {
                throw new ArgumentException("The FFmpegRatio numerator must be greater than zero.", "numerator");
            }
            if (denominator <= 0)
            {
                throw new ArgumentException("The FFmpegRatio denominator must be greater than zero.", "denominator");
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

        public static FFmpegRatio Create(decimal aspectRatio)
        {
            return new FFmpegRatio(aspectRatio);
        }

        public static FFmpegRatio Create(int numerator, int denominator)
        {
            return new FFmpegRatio(numerator, denominator);
        }
    }
}
