using System;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Validators
{
    public class TimeSpanGreterThanZeroValidator : IValidator
    {
        public bool Validate(object value)
        {
            return ((TimeSpan) value) > TimeSpan.FromSeconds(0);
        }
    }
}
