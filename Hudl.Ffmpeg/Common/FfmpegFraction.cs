using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Common
{
    public class FfmpegFraction : IFfprobeValue
    {
        private FfmpegFraction(int numerator, int denominator)
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
            var ffprobeFraction = obj as FfprobeFraction;
            if (ffprobeFraction == null)
            {
                return false;
            }

            return ffprobeFraction.Numerator == Numerator &&
                   ffprobeFraction.Denominator == Denominator;
        }

        public static FfmpegFraction Create(int numerator, int denominator)
        {
            return new FfmpegFraction(numerator, denominator);
        }

        internal static FfmpegFraction Create(FfprobeFraction ffprobeFraction)
        {
            if (ffprobeFraction == null)
            {
                return null;
            }

            return new FfmpegFraction(ffprobeFraction.Numerator, ffprobeFraction.Denominator);
        }
    }
}
