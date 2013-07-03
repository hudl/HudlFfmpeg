using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// helper class that helps with validation of objects in a ffmpeg project
    /// </summary>
    public class Validate
    {
        /// <summary>
        /// returns a boolean indicating if <cref name="TObject"/> is applicable to <cref name="TRestrictedTo"/> 
        /// </summary>
        /// <typeparam name="TObject">the type in question to be applied to</typeparam>
        /// <typeparam name="TRestrictedTo">the type in question that is required</typeparam>
        public static bool AppliesTo<TObject, TRestrictedTo>()
            where TRestrictedTo : IResource
        {
            var objectType = typeof (TObject);
            var restrictedType = typeof (TRestrictedTo);
            return AppliesTo(objectType, restrictedType);
        }

        /// <summary>
        /// returns a boolean indicating if <cref name="objectType"/> is applicable to <cref name="restrictedType"/> 
        /// </summary>
        public static bool AppliesTo(Type objectType, Type restrictedType)
        {
            var attributeType = typeof(AppliesToResourceAttribute);

            var allAttributes = new List<CustomAttributeData>(objectType.CustomAttributes);
            var matchingAttributes = allAttributes.FindAll(a => a.AttributeType == attributeType);
            if (matchingAttributes.Count == 0)
            {
                return false;
            }

            var appliesTo = matchingAttributes.Any(attribute =>
            {
                var namedArguments = attribute.NamedArguments;
                if (namedArguments == null) return false;
                return namedArguments.Any(argument =>
                {
                    if (argument.MemberName != "Type") return false;
                    var namedArgType = (argument.TypedValue.Value as Type);
                    return namedArgType != null &&
                           (namedArgType == restrictedType ||
                            namedArgType.IsAssignableFrom(restrictedType));
                });
            });
            return appliesTo; 
        }

        public static bool IsSettingFor<TSetting>(TSetting item, SettingsCollectionResourceTypes type)
            where TSetting : ISetting
        {
            var objectType = item.GetType();
            var attributeType = typeof (SettingsApplicationAttribute);

            var allAttributes = new List<CustomAttributeData>(objectType.CustomAttributes);
            var matchingAttribute = allAttributes.FirstOrDefault(a => a.AttributeType == attributeType);
            if (matchingAttribute == null || matchingAttribute.NamedArguments == null)
            {
                return false;
            }

            var resourceTypeArgument = matchingAttribute.NamedArguments
                                                         .FirstOrDefault(a => a.MemberName == "ResourceType");
            var resourceTypeValue = (SettingsCollectionResourceTypes)resourceTypeArgument.TypedValue.Value;
            return (type == resourceTypeValue);
        }

        internal static SettingsApplicationData GetSettingData<TSetting>()
            where TSetting : ISetting
        {
            var typeAAttributes = new List<CustomAttributeData>(typeof (TSetting).CustomAttributes);
            var typeASettingsAttributes = typeAAttributes.FirstOrDefault(a => a.AttributeType == typeof (SettingsApplicationAttribute));
            if (typeASettingsAttributes == null || typeASettingsAttributes.NamedArguments == null)
            {
                return null;
            }
            var dataArgument = typeASettingsAttributes.NamedArguments
                                                      .FirstOrDefault(a => a.MemberName == "Data");
            return (SettingsApplicationData)dataArgument.TypedValue.Value;
        }

        internal static SettingsApplicationData GetSettingData<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            return GetSettingData<TSetting>();
        }

        internal static Dictionary<Type, SettingsApplicationData> GetSettingCollectionData(SettingsCollection collection)
        {
            var settingsDictionary = new Dictionary<Type, SettingsApplicationData>();
            collection.SettingsList.ForEach(setting =>
                {
                    var settingsType = setting.GetType();
                    if (settingsDictionary.ContainsKey(settingsType)) return;
                    settingsDictionary.Add(settingsType, GetSettingData(setting));
                });
            return settingsDictionary;
        }
    }
}
