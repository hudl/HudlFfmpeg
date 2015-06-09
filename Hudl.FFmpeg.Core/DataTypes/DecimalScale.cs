using System;
using System.Globalization;

namespace Hudl.FFmpeg.DataTypes
{
    /// <summary>
    /// Describes an intensity scale that is used to intensify settings within ffmpeg, between -1 to 1
    /// </summary>
    public class DecimalScale
    {
        private const decimal MinValue = -1;
        private const decimal MaxValue = 1;
        private decimal? _value;

        public DecimalScale()
        {
        }
        public DecimalScale(decimal value)
            : this()
        {
            Value = value;
        }

        public decimal? Value
        {
            get
            {
                return _value; 
            }
            set
            {
                if (value != null && (value > 1 || value < -1))
                {
                    throw new InvalidOperationException(string.Format("Ratio must be a decimal value {0} and {1}", MinValue, MaxValue));
                }
                _value = value;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool IsNullOrZero(DecimalScale scale)
        {
            return (scale == null || scale.Value == 0); 
        }

        public static implicit operator DecimalScale(decimal value)
        {
            return new DecimalScale(value);
        }
    }
}
