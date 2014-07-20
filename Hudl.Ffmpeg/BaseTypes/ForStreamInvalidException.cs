using System;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// invalid applies to exception, thrown when Applier does not apply to AppliedTo
    /// </summary>
    public class ForStreamInvalidException: Exception
    {
        public ForStreamInvalidException(Type applier, Type appliedTo)
            : base(string.Format("Type of '{0}' does not apply to type of '{1}'",
                   applier.Name,
                   appliedTo.Name))
        {
        }
    }
}
