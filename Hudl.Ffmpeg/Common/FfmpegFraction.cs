using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Common
{
    public class FFmpegFraction : IFFprobeValue
    {
        private FFmpegFraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public int Numerator { get; set; }

        public int Denominator { get; set; }

        public double ToDouble()
        {
            return Numerator / Denominator; 
        }

        public override bool Equals(object obj)
        {
            var ffprobeFraction = obj as FFprobeFraction;
            if (ffprobeFraction == null)
            {
                return false;
            }

            return ffprobeFraction.Numerator == Numerator &&
                   ffprobeFraction.Denominator == Denominator;
        }

        public static FFmpegFraction Create(int numerator, int denominator)
        {
            return new FFmpegFraction(numerator, denominator);
        }

        internal static FFmpegFraction Create(FFprobeFraction ffprobeFraction)
        {
            if (ffprobeFraction == null)
            {
                return null;
            }

            return new FFmpegFraction(ffprobeFraction.Numerator, ffprobeFraction.Denominator);
        }
    }
}
