using System;

namespace Hudl.FFmpeg.Filters.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FilterAttribute : Attribute
    {
        public FilterAttribute()
        {
        }

        public string Name { get; set; }

        public int MinInputs { get; set; }

        public int MaxInputs { get; set; }
    }
}
