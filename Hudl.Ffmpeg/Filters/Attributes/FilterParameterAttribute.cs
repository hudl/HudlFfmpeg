using System;
using Hudl.FFmpeg.Filters.BaseTypes;

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

        public ValidatorType Validator { get; set; }

        public Type Formatter { get; set; } 
    }

    public abstract class ValidatorType 
    {
    }
}
