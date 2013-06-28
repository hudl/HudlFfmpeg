using System;
using System.Globalization;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Describes an intensity scale that is used to intensify settings within ffmpeg, between -1 to 1
    /// </summary>
    public class FfmpegScale
    {
        private const decimal MinValue = -1;
        private const decimal MaxValue = 1;
        private decimal _value;

        public FfmpegScale()
        {
        }
        public FfmpegScale(decimal value)
            : this()
        {
            Value = value;
        }

        public decimal Value
        {
            get
            {
                return _value; 
            }
            set
            {
                if (value > 1 || value < -1)
                {
                    throw new InvalidOperationException(string.Format("Ratio must be a decimal value {0} and {1}", MinValue, MaxValue));
                }
                _value = value;
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public static bool IsNullOrZero(FfmpegScale scale)
        {
            return (scale == null || scale.Value == 0); 
        }
    }
}
