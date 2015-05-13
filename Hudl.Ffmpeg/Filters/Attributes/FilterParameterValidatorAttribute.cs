using System;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.BaseTypes;

namespace Hudl.FFmpeg.Filters.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FilterParameterValidatorAttribute : Attribute
    {
        public FilterParameterValidatorAttribute(IFilterParameterValidator validator)
        {
            Validator = validator;
        }

        public FilterParameterValidatorAttribute(LogicalOperators op, object value)
        {
            Validator = SimpleFilterValidator.Create(op, value);
        }

        public IFilterParameterValidator Validator { get; set; }
    }
}
