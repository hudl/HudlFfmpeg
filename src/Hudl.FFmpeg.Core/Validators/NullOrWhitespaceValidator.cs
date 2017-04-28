using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Validators
{
    public class NullOrWhitespaceValidator : IValidator
    {
        public bool Validate(object value)
        {
            return !string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}
