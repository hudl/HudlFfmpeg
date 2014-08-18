using System;

namespace Hudl.FFmpeg.BaseTypes
{
    /// <summary>
    /// class level attribute that sets up a connection between a type with a resource
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    public class ForStreamAttribute : Attribute
    {
        public Type Type { get; set; }
    }
}
