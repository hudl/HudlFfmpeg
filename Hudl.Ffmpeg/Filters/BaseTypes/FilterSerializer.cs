using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public class FilterSerializer
    {
        public static string SerializeFilter(IFilter filter)
        {
            var filterData = ReadFilterData(filter);

        }

        private static FilterData ReadFilterData(IFilter filter)
        {
            var filterType = filter.GetType();

            return new FilterData
                {
                    FilterAttribute = ReadFilterAttribute(filter, filterType),
                    FilterParameterProperties = ReadAllFilterParameters(filter, filterType)
                };
        }
        private static FilterAttribute ReadFilterAttribute(IFilter filter, Type filterType)
        {
            var filterAttribute = (FilterAttribute) Attribute.GetCustomAttribute(filterType, typeof (FilterAttribute));

            if (filterAttribute == null)
            {
                throw new Exception("Cannot serialize a filter that does not contain the FilterAttribute on the class.");
            }

            return filterAttribute;
        }
        private static List<FilterParameterPropertyData> ReadAllFilterParameters(IFilter filter, Type filterType)
        {
            var filterParameterDataList = new List<FilterParameterPropertyData>();
            var properties = new List<PropertyInfo>(filterType.GetProperties());

            foreach (var propertyInfo in properties)
            {
                var filterParameterValue = propertyInfo.GetValue(filter);
                var filterParameterAttribute = (FilterParameterAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(FilterParameterAttribute));
                var filterParameterValidationAttribute = (FilterParameterValidatorAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(FilterParameterValidatorAttribute));

                if (filterParameterAttribute == null)
                {
                    continue;
                }

                filterParameterDataList.Add(new FilterParameterPropertyData
                    {
                        Name = propertyInfo.Name,
                        Value = filterParameterValue, 
                        ParameterAttribute = filterParameterAttribute, 
                        ParameterValidatorAttribute = filterParameterValidationAttribute
                    });
            }

            return filterParameterDataList; 
        }

        private static void RunFilterValidation(FilterData filterData)
        {
            var isValid = filterData.FilterParameterProperties
                                    .Where(fp => fp.ParameterValidatorAttribute != null)
                                    .All(fp =>
                                        {
                                            var parameterValid = fp.ParameterValidatorAttribute.Validator.Validate(fp.Value);

                                            if (!parameterValid)
                                            {
                                                throw new Exception(string.Format("Filter parameter \"{0}\" is not valid, value must be {1}", fp.Name));
                                            }
                                        });

        } 
    }

    public class FilterData
    {
        public FilterData()
        {
            FilterParameterProperties = new List<FilterParameterPropertyData>();
        }

        public FilterAttribute FilterAttribute { get; set; }

        public List<FilterParameterPropertyData> FilterParameterProperties { get; set; } 
    }

    public class FilterParameterPropertyData
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public FilterParameterAttribute ParameterAttribute { get; set; }

        public FilterParameterValidatorAttribute ParameterValidatorAttribute { get; set; }
    }
}
