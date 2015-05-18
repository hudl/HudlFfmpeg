using System;
using System.Linq;
using System.Reflection;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Serialization
{
    internal class FilterSerializerAttributeParser
    {
        public static FilterSerializerData GetFilterSerializerData(IFilter filter)
        {
            var filterType = filter.GetType();
            var filterSerializerData = new FilterSerializerData();

            FillFilterAttribute(filterSerializerData, filter, filterType);

            FillFilterParameterAttributes(filterSerializerData, filter, filterType);

            return filterSerializerData;
        }

        private static void FillFilterAttribute(FilterSerializerData filterSerializerData, IFilter filter, Type filterType)
        {
            var filterParameter = (FilterAttribute)Attribute.GetCustomAttribute(filterType, typeof(FilterAttribute));
            if (filterParameter == null)
            {
                throw new Exception(string.Format("IFilter type of \"{0}\", does not contain the FilterAttribute and must.", filterType.Name));
            }

            filterSerializerData.Filter = filterParameter; 
        }

        private static void FillFilterParameterAttributes(FilterSerializerData filterSerializerData, IFilter filter, Type filterType)
        {
            var filterProperties = filterType.GetProperties().ToList();

            foreach (var filterProperty in filterProperties)
            {
                var filterPropertyAttribute = (FilterParameterAttribute)Attribute.GetCustomAttribute(filterProperty, typeof (FilterParameterAttribute));
                if (filterPropertyAttribute == null)
                {
                    continue;
                }

                var filterPropertyValue = filterProperty.GetValue(filter);

                var filterPropertyValidationAttribute = (FilterParameterValidatorAttribute)Attribute.GetCustomAttribute(filterProperty, typeof (FilterParameterValidatorAttribute));
                if (filterPropertyValidationAttribute != null)
                {
                    RunFilterSerializationValidation(filterPropertyValidationAttribute, filterType, filterProperty, filterPropertyValue);
                }

                var filterPropertyFormattedValue = RunFilterSerializationFormat(filterPropertyAttribute, filterType, filterProperty, filterPropertyValue);

                filterSerializerData.Parameters.Add(new FilterSerializerDataParameter
                {
                    Name = filterProperty.Name,
                    Value = filterPropertyFormattedValue,
                    Parameter = filterPropertyAttribute,
                    IsDefault = filterPropertyAttribute.Default == filterPropertyValue
                });
            }
        }

        private static void RunFilterSerializationValidation(FilterParameterValidatorAttribute filterPropertyValidationAttribute, Type filterType, PropertyInfo propertyInfo, object value)
        {
            if (!filterPropertyValidationAttribute.Vaildate())
            {
                throw new Exception(string.Format("IFilter type of \"{0}\" parameter \"{1}\", Formatter must be {2} {3}.", filterType.Name, propertyInfo.Name, filterPropertyValidationAttribute.Op, filterPropertyValidationAttribute.Value));
            }
        }

        private static string RunFilterSerializationFormat(FilterParameterAttribute filterParameterAttribute, Type filterType, PropertyInfo propertyInfo, object value)
        {
            if (value == null)
            {
                return string.Empty; 
            }

            if (filterParameterAttribute.Formatter == null)
            {
                return value.ToString();
            }

            if (!typeof(IFilterParameterFormatter).IsAssignableFrom(filterParameterAttribute.Formatter))
            {
                throw new Exception(string.Format("IFilter type of \"{0}\" parameter \"{1}\", Formatter must be of type IFilterParameterFormatter.", filterType.Name, propertyInfo.Name));
            }

            var formatterFromAttribute = (IFilterParameterFormatter)Activator.CreateInstance(filterParameterAttribute.Formatter);

            return formatterFromAttribute.Format(value);
        }

    }
}
