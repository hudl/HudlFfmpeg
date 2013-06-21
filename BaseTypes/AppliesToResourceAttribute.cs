using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// class level attribute that sets up a connection between a type with a resource
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    class AppliesToResourceAttribute : Attribute
    {
        public Type Type { get; set; }
    }
}
