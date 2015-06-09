using System;

namespace Hudl.FFmpeg.DataTypes
{
    public abstract class TwoPartNumericalBase
    {
        private readonly char _separationCharacter;

        protected TwoPartNumericalBase(char separationChar)
        {
            _separationCharacter = separationChar;
        }

        public int Numerator { get; set; }

        public int Denominator { get; set; }

        public double ToDouble()
        {
            return (double)Numerator / (double)Denominator; 
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Numerator, _separationCharacter, Denominator);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var FFprobeFraction = obj as TwoPartNumericalBase;
            if (FFprobeFraction == null)
            {
                return false;
            }

            return FFprobeFraction.Numerator == Numerator &&
                   FFprobeFraction.Denominator == Denominator;
        }

        public static bool TryParse<TObjectType>(string rawValue, out TObjectType value)
            where TObjectType : TwoPartNumericalBase, new()
        {
            try
            {
                if (rawValue == null)
                {
                    value = null;
                    return false;
                }

                var splitValues = rawValue.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

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

                value = new TObjectType
                {
                    Numerator = numerator,
                    Denominator = denominator
                };

                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }
    }
}
