using System;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public class SimpleFilterValidator : IFilterParameterValidator
    {
        private SimpleFilterValidator (LogicalOperators op, object value)
        {
            Op = op;
            Value = value;
        }

        public LogicalOperators Op { get; set; }

        public object Value { get; set; }

        public bool Validate(object value)
        {
            throw new NotImplementedException();
        }

        public static SimpleFilterValidator Create(LogicalOperators op, object value)
        {
            return new SimpleFilterValidator(op, value);
        }
        public static SimpleFilterValidator EqualTo(object value)
        {
            return new SimpleFilterValidator(LogicalOperators.Equals, value);
        }
        public static SimpleFilterValidator NotEqualTo(object value)
        {
            return new SimpleFilterValidator(LogicalOperators.NotEquals, value);
        }
        public static SimpleFilterValidator GreaterThan(object value)
        {
            return new SimpleFilterValidator(LogicalOperators.GreaterThan, value);
        }
        public static SimpleFilterValidator GreaterThanOrEqualTo(object value)
        {
            return new SimpleFilterValidator(LogicalOperators.GreaterThanOrEqual, value);
        }
        public static SimpleFilterValidator LesserThan(object value)
        {
            return new SimpleFilterValidator(LogicalOperators.LesserThan, value);
        }
        public static SimpleFilterValidator LesserThanOrEqualTo(object value)
        {
            return new SimpleFilterValidator(LogicalOperators.LesserThanOrEqual, value);
        }
    }
}
