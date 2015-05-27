using System;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ValidateAttribute : Attribute
    {
        public ValidateAttribute(LogicalOperators op, object value)
        {
            Op = op;
            Value = new [] {value};
        }

        public ValidateAttribute(LogicalOperators op, params object[] values)
        {
            Op = op;
            Value = values;
        }

        public LogicalOperators Op { get; set; }

        public object[] Value { get; set; }

        internal bool Vaildate()
        {
            return true;
        }
    }
}
