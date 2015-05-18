using System;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Filters.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FilterParameterValidatorAttribute : Attribute
    {
        public FilterParameterValidatorAttribute(LogicalOperators op, object value)
        {
            Op = op;
            Value = value;
        }

        public LogicalOperators Op { get; set; }

        public object Value { get; set; }

        internal bool Vaildate()
        {
            return true;
        }
    }
}
