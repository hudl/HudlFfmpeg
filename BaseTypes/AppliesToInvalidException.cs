using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// invalid applies to exception, thrown when <see cref="TypeA"/> does not apply to <see cref="TypeB"/>
    /// </summary>
    /// <typeparam name="TypeA">the type that is to be applied</typeparam>
    /// <typeparam name="TypeB">the type that is applied to</typeparam>
    public class AppliesToInvalidException<TypeA, TypeB> : Exception
        where TypeB : IResource
    {
        public AppliesToInvalidException()
            : base(string.Format("Type of '{0}' does not apply to type of '{1}'", 
                   typeof(TypeA).Name, 
                   typeof(TypeB).Name))
        {
        }
    }
}
