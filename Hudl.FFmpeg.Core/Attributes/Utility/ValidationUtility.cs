using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Extensions;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Interfaces;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Attributes.Utility
{
    public class ValidationUtility
    {
        public static bool Validate(IFilter filter)
        {
            var filterType = filter.GetType();
            var filterParameters =
                filterType.GetProperties()
                          .Where(p => AttributeRetrieval.GetAttributes<ValidateAttribute>(p).Any())
                          .Select(p => new { Info = p, Value = p.GetValue(filter)})
                          .ToList();

            return filterParameters.All(p => Validate(filterType, p.Info, p.Value));
        }

        public static bool Validate(ISetting setting)
        {
            var settingType = setting.GetType();
            var settingParameters =
                settingType.GetProperties()
                          .Where(p => AttributeRetrieval.GetAttributes<ValidateAttribute>(p).Any())
                          .Select(p => new { Info = p, Value = p.GetValue(setting) })
                          .ToList();

            return settingParameters.All(p => Validate(settingType, p.Info, p.Value));
        }

        public static bool Validate(Type objectType, PropertyInfo propertyInfo, object value)
        {
            var validateAttributes = AttributeRetrieval.GetAttributes<ValidateAttribute>(propertyInfo);
            return validateAttributes == null || !validateAttributes.Any() || validateAttributes.All(va => ValidateSingle(propertyInfo, va, value));
        }

        internal static bool ValidateSingle(PropertyInfo propertyInfo, ValidateAttribute validate, object value)
        {
            if (value == null)
            {
                return true;
            }

            if (validate.Validator != null && typeof(IValidator).IsAssignableFrom(validate.Validator))
            {
                var validator = (IValidator) Activator.CreateInstance(validate.Validator);
                return validator.Validate(value);
            }

            return value.IsNumeric()
                ? ValidateNumerics(propertyInfo, validate, value)
                : ValidateNonNumerics(propertyInfo, validate, value);
        }

        internal static bool ValidateNumerics(PropertyInfo propertyInfo, ValidateAttribute validate, object value)
        {
            var success = true; 
            var decimalRepresentationOfValidate = 0m;
            var decimalRepresentationOfObject = 0m;

            success &= decimal.TryParse(value.ToString(), out decimalRepresentationOfObject);
            success &= decimal.TryParse(validate.Value.First().ToString(), out decimalRepresentationOfValidate);

            if (!success)
            {
                return false; 
            }

            switch (validate.Op)
            {
                case LogicalOperators.Equals:
                    return decimalRepresentationOfObject == decimalRepresentationOfValidate;
                case LogicalOperators.NotEquals:
                    return decimalRepresentationOfObject != decimalRepresentationOfValidate;
                case LogicalOperators.LesserThan:
                    return decimalRepresentationOfObject < decimalRepresentationOfValidate;
                case LogicalOperators.LesserThanOrEqual:
                    return decimalRepresentationOfObject <= decimalRepresentationOfValidate;
                case LogicalOperators.GreaterThan:
                    return decimalRepresentationOfObject > decimalRepresentationOfValidate;
                case LogicalOperators.GreaterThanOrEqual:
                    return decimalRepresentationOfObject >= decimalRepresentationOfValidate;
                case LogicalOperators.IsNotOneOf:
                    return !validate.Value.Any(v =>
                        {
                            if (!decimal.TryParse(validate.Value.First().ToString(), out decimalRepresentationOfObject))
                            {
                                return false;
                            }

                            return decimalRepresentationOfObject == decimalRepresentationOfValidate;
                        });
                case LogicalOperators.IsOneOf:
                    return validate.Value.Any(v =>
                        {
                            if (!decimal.TryParse(validate.Value.First().ToString(), out decimalRepresentationOfObject))
                            {
                                return false;
                            }

                            return decimalRepresentationOfObject == decimalRepresentationOfValidate;
                        });
            }

            throw new InvalidOperationException(string.Format("Validation for \"{0}\" of type \"{1}\", is not valid for numeric types.", propertyInfo.Name, validate.Op));
        }

        internal static bool ValidateNonNumerics(PropertyInfo propertyInfo, ValidateAttribute validate, object value)
        {
            switch (validate.Op)
            {
                case LogicalOperators.Equals:
                    return value.Equals(validate.Value.First()); 
                case LogicalOperators.NotEquals:
                    return !value.Equals(validate.Value.First());
                case LogicalOperators.IsOneOf:
                    return validate.Value.Any(value.Equals);
                case LogicalOperators.IsNotOneOf:
                    return !validate.Value.Any(value.Equals);
            }

            throw new InvalidOperationException(string.Format("Validation for \"{0}\" of type \"{1}\", is not valid for non numeric types.", propertyInfo.Name, validate.Op));
        }
    }
}
