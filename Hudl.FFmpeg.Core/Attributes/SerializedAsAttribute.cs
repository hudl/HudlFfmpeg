using System;

namespace Hudl.FFmpeg.Attributes
{
    /// <summary>
    /// enum level attribute that allows a serialized name to be pulled from an attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple=true, Inherited=true)]
    public class SerializedAsAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
