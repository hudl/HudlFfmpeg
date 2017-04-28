using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Attributes.Utility;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Filters.Serialization
{
    internal class FilterSerializerAttributeParser
    {
        public static FilterSerializerData GetFilterSerializerData(IFilter filter, FilterBindingContext context)
        {
            var filterType = filter.GetType();
            var filterSerializerData = new FilterSerializerData();

            FillFilterAttribute(filterSerializerData, filter, filterType);

            FillFilterParameterAttributes(filterSerializerData, filter, filterType, context);

            return filterSerializerData;
        }

        private static void FillFilterAttribute(FilterSerializerData filterSerializerData, IFilter filter, Type filterType)
        {
            var filterParameter = AttributeRetrieval.GetAttribute<FilterAttribute>(filterType); 
            if (filterParameter == null)
            {
                throw new Exception(string.Format("IFilter type of \"{0}\", does not contain the FilterAttribute and must.", filterType.Name));
            }

            filterSerializerData.Filter = filterParameter; 
        }

        private static void FillFilterParameterAttributes(FilterSerializerData filterSerializerData, IFilter filter, Type filterType, FilterBindingContext context)
        {
            var propertyInfos = filterType.GetProperties().ToList();

            foreach (var propertyInfo in propertyInfos)
            {
                var filterParameterAttribute = AttributeRetrieval.GetAttribute<FilterParameterAttribute>(propertyInfo);
                if (filterParameterAttribute == null)
                {
                    continue;
                }

                //get the value set or bound to the parameter
                var value = GetFilterSerializationBindingValue(filterParameterAttribute, propertyInfo, context, filter);

                //run validation on the parameter to ensure that it meets all standards
                RunFilterSerializationValidation(filterType, propertyInfo, value);

                //run formatter on the validated value
                var filterPropertyFormattedValue = RunFilterSerializationFormat(filterParameterAttribute, filterType, propertyInfo, value);

                var filterPropertyIsDefault = filterParameterAttribute.Default != null
                                              && value != null
                                              && filterParameterAttribute.Default.Equals(value);

                filterSerializerData.Parameters.Add(new FilterSerializerDataParameter
                {
                    Name = filterParameterAttribute.Name,
                    Value = filterPropertyFormattedValue,
                    Parameter = filterParameterAttribute,
                    IsDefault = filterPropertyIsDefault
                });
            }
        }

        private static object GetFilterSerializationBindingValue(FilterParameterAttribute filterParameterAttribute, PropertyInfo propertyInfo, FilterBindingContext context, IFilter filter)
        {
            if (filterParameterAttribute.Binding == null)
            {
                return propertyInfo.GetValue(filter);
            }

            var bindingObject = (IFilterParameterBinding)Activator.CreateInstance(filterParameterAttribute.Binding);
            
            return bindingObject.GetValue(context);
        }

        private static void RunFilterSerializationValidation(Type filterType, PropertyInfo propertyInfo, object value)
        {
            if (!ValidationUtility.Validate(filterType, propertyInfo, value))
            {
                throw new InvalidOperationException(string.Format("IFilter type of \"{0}\" parameter \"{1}\" has an invalid value of \"{2}\".", filterType.Name, propertyInfo.Name, value));
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
                decimal decValue;
                return Decimal.TryParse(value.ToString(), out decValue) ? decValue.ToString(CultureInfo.InvariantCulture) : value.ToString();
            }

            if (!typeof(IFormatter).IsAssignableFrom(filterParameterAttribute.Formatter))
            {
                throw new Exception(string.Format("IFilter type of \"{0}\" parameter \"{1}\", Formatter must be of type IFilterParameterFormatter.", filterType.Name, propertyInfo.Name));
            }

            var formatterFromAttribute = (IFormatter)Activator.CreateInstance(filterParameterAttribute.Formatter);

            return formatterFromAttribute.Format(value);
        }

    }
}
