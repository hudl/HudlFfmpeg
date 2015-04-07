namespace Hudl.FFmpeg.Common.DataTypes
{
    public class Ratio : TwoPartNumericalBase
    {
        public Ratio()
            : base(':')
        {
        }

        public Ratio(int numerator, int denominator)
            : base (':')
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public string ToFractionalString()
        {
            return string.Format("{0}/{1}", Numerator, Denominator); 
        }

        public static bool TryParse(string rawValue, out Ratio value)
        {
            return TryParse<Ratio>(rawValue, out value); 
        }

        public static Ratio Create(int numerator, int denominator)
        {
            return new Ratio(numerator, denominator);
        }
    }
}
