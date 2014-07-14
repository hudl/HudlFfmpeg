using System;

namespace Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes
{
    internal class FfprobeFraction : IFfprobeValue
    {
        private FfprobeFraction(int numerator, int denominator)
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

        public static bool TryParse(FfprobeObject rawValue, out FfprobeFraction value)
        {
            try
            {
                if (rawValue == null)
                {
                    value = null;
                    return false;
                }

                var splitValues = rawValue.Value.ToString().Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries);

                var numerator = 0;
                var denominator = 0;
                if (!int.TryParse(splitValues[0], out numerator))
                {
                    value = null;
                    return false;
                }
                if (!int.TryParse(splitValues[1], out denominator))
                {
                    value = null;
                    return false;
                }

                value = new FfprobeFraction(numerator, denominator);

                return true;
            }
            catch (Exception err)
            {
                value = null;
                return false;
            }
        }
    }
}
