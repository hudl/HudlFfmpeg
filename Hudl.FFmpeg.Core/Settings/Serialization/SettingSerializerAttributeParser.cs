using System;
using System.Linq;
using System.Reflection;
using Hudl.FFmpeg.Attributes;
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

            FillFilterParameterAttributes(settingSerializerData, setting, settingType);

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

        private static void FillFilterParameterAttributes(SettingSerializerData settingSerializerData, ISetting setting, Type settingType)
        {
            var settingProperties = settingType.GetProperties().ToList();

            foreach (var settingProperty in settingProperties)
            {
                var settingValueAttribute = AttributeRetrieval.GetAttribute<SettingValueAttribute>(settingProperty);
                if (settingValueAttribute == null)
                {
                    continue;
                }

                var settingPropertyValue = settingProperty.GetValue(setting);

                var settingPropertyValidationAttribute = (ValidateAttribute)Attribute.GetCustomAttribute(settingProperty, typeof (ValidateAttribute));
                if (settingPropertyValidationAttribute != null)
                {
                    RunFilterSerializationValidation(settingPropertyValidationAttribute, settingType, settingProperty, settingPropertyValue);
                }

                var settingPropertyFormattedValue = RunFilterSerializationFormat(settingValueAttribute, settingType, settingProperty, settingPropertyValue);
                
                settingSerializerData.Value = new SettingSerializerDataValue
                {
                    Value = settingPropertyFormattedValue,
                    Parameter = settingValueAttribute,
                };
            }
        }

        private static void RunFilterSerializationValidation(ValidateAttribute settingsValidateAttribute, Type filterType, PropertyInfo propertyInfo, object value)
        {
            if (!settingsValidateAttribute.Vaildate())
            {
                throw new Exception(string.Format("ISetting type of \"{0}\" value \"{1}\", Formatter must be {2} {3}.", filterType.Name, propertyInfo.Name, settingsValidateAttribute.Op, settingsValidateAttribute.Value));
            }
        }

        private static string RunFilterSerializationFormat(SettingValueAttribute settingValueAttribute, Type filterType, PropertyInfo propertyInfo, object value)
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
