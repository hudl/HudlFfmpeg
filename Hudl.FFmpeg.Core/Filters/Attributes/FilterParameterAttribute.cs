using System;

namespace Hudl.FFmpeg.Filters.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FilterParameterAttribute : Attribute
    {
        public FilterParameterAttribute()
        {
        }
        public FilterParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public object Default { get; set; }

        public Type Formatter { get; set; } 
    }
}
