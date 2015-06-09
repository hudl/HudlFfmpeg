using System;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ValidateAttribute : Attribute
    {
        public ValidateAttribute(Type validator)
        {
            Validator = validator;
        }

        public ValidateAttribute(LogicalOperators op, object value)
        {
            Op = op;
            Value = new [] {value};
        }

        public ValidateAttribute(LogicalOperators op, params object[] values)
        {
            Op = op;
            Value = values ?? new[] {(object) null};
        }

        public Type Validator { get; set; }

        public LogicalOperators? Op { get; set; }

        public object[] Value { get; set; }

    }
}
