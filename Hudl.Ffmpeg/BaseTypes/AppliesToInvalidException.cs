using System;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// invalid applies to exception, thrown when Applier does not apply to AppliedTo
    /// </summary>
    public class AppliesToInvalidException: Exception
    {
        public AppliesToInvalidException(Type applier, Type appliedTo)
            : base(string.Format("Type of '{0}' does not apply to type of '{1}'",
                   applier.Name,
                   appliedTo.Name))
        {
        }
    }
}
