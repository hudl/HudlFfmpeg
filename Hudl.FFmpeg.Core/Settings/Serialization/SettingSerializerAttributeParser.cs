using System;
using System.Linq;
using System.Reflection;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Attributes.Utility;
using Hudl.FFmpeg.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.Serialization
{
    internal class SettingSerializerAttributeParser
    {
        public static SettingSerializerData GetSettingSerializerData(ISetting setting)
        {
            var settingType = setting.GetType();
            var settingSerializerData = new SettingSerializerData();

            FillSettingAttribute(settingSerializerData, setting, settingType);

            FillSettingParameterAttributes(settingSerializerData, setting, settingType);

            return settingSerializerData;
        }

        private static void FillSettingAttribute(SettingSerializerData settingSerializerData, ISetting setting, Type settingType)
        {
            var settingParameter = AttributeRetrieval.GetAttribute<SettingAttribute>(settingType);
            if (settingParameter == null)
            {
                throw new Exception(string.Format("ISetting type of \"{0}\", does not contain the FilterAttribute and must.", settingParameter.Name));
            }

            settingSerializerData.Setting = settingParameter; 
        }

        private static void FillSettingParameterAttributes(SettingSerializerData settingSerializerData, ISetting setting, Type settingType)
        {
            var propertyInfos = settingType.GetProperties().ToList();

            foreach (var propertyInfo in propertyInfos)
            {
                var settingParameterAttribute = AttributeRetrieval.GetAttribute<SettingParameterAttribute>(propertyInfo);
                if (settingParameterAttribute == null)
                {
                    continue;
                }

                var settingPropertyValue = propertyInfo.GetValue(setting);

                RunSettingSerializationValidation(settingType, propertyInfo, settingPropertyValue);

                var settingPropertyFormattedValue = RunSettingSerializationFormat(settingParameterAttribute, settingType, propertyInfo, settingPropertyValue);
                
                settingSerializerData.Parameters.Add(new SettingSerializerDataParameter
                {
                    Value = settingPropertyFormattedValue,
                    Parameter = settingParameterAttribute,
                });
            }
        }

        private static void RunSettingSerializationValidation(Type filterType, PropertyInfo propertyInfo, object value)
        {
            if (!ValidationUtility.Validate(filterType, propertyInfo, value))
            {
                throw new InvalidOperationException(string.Format("ISetting type of \"{0}\" parameter \"{1}\" has an invalid value of \"{2}\".", filterType.Name, propertyInfo.Name, value));
            }
        }

        private static string RunSettingSerializationFormat(SettingParameterAttribute settingValueAttribute, Type filterType, PropertyInfo propertyInfo, object value)
        {
            if (value == null)
            {
                return string.Empty; 
            }

            if (settingValueAttribute.Formatter == null)
            {
                return value.ToString();
            }

            if (!typeof(IFormatter).IsAssignableFrom(settingValueAttribute.Formatter))
            {
                throw new Exception(string.Format("ISetting type of \"{0}\" parameter \"{1}\", Formatter must be of type IFormatter.", filterType.Name, propertyInfo.Name));
            }

            var formatterFromAttribute = (IFormatter)Activator.CreateInstance(settingValueAttribute.Formatter);

            return formatterFromAttribute.Format(value);
        }

    }
}
