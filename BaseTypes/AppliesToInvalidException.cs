using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// invalid applies to exception, thrown when <see cref="TApply"/> does not apply to <see cref="TAppliedTo"/>
    /// </summary>
    /// <typeparam name="TApply">the type that is to be applied</typeparam>
    /// <typeparam name="TAppliedTo">the type that is applied to</typeparam>
    public class AppliesToInvalidException<TApply, TAppliedTo> : Exception
        where TAppliedTo : IResource
    {
        public AppliesToInvalidException()
            : base(string.Format("Type of '{0}' does not apply to type of '{1}'",
                   typeof(TApply).Name,
                   typeof(TAppliedTo).Name))
        {
        }
    }
}
