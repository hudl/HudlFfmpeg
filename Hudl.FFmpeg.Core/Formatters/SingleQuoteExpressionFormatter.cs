using Hudl.FFmpeg.Interfaces;
using System.Text.RegularExpressions;

namespace Hudl.FFmpeg.Formatters
{
    public class SingleQuoteExpressionFormatter : IFormatter
    {
        public string Format(object value)
        {
            var valueAsString = value.ToString();
            var valueIsNumeric = Regex.IsMatch(valueAsString, @"^\d+$");
            return (!valueIsNumeric)
                ? $"'{valueAsString}'"
                : valueAsString; 
        }
    }
}
