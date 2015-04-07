namespace Hudl.FFmpeg.Common.DataTypes
{
    public class Fraction : TwoPartNumericalBase
    {
        public Fraction()
            : base('/')
        {
        }

        public Fraction(int numerator, int denominator)
            : base('/')
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static bool TryParse(string rawValue, out Fraction value)
        {
            return TryParse<Fraction>(rawValue, out value); 
        }

        public static Fraction Create(int numerator, int denominator)
        {
            return new Fraction(numerator, denominator);
        }
    }
}
