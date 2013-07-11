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
            var matchingAttributes = GetAttributes<AppliesToResourceAttribute>(objectType);
            if (matchingAttributes.Count == 0)
            {
                return false;
            }

            return matchingAttributes.Any(attribute => (attribute.Type == restrictedType ||
                                                        attribute.Type.IsAssignableFrom(restrictedType)));
        }

        public static bool IsSettingFor<TSetting>(TSetting item, SettingsCollectionResourceType type)
            where TSetting : ISetting
        {
            var itemType = item.GetType();
            var matchingAttribute = GetAttribute<SettingsApplicationAttribute>(itemType);
            
            return matchingAttribute != null && type == matchingAttribute.ResourceType;
        }

        internal static SettingsApplicationData GetSettingData<TSetting>()
            where TSetting : ISetting
        {
            return GetSettingData(typeof(TSetting));
        }

        internal static SettingsApplicationData GetSettingData<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            return GetSettingData(setting.GetType());
        }

        internal static SettingsApplicationData GetSettingData(Type itemType)
        {
            var matchingAttribute = GetAttribute<SettingsApplicationAttribute>(itemType);

            return matchingAttribute == null ? null : matchingAttribute.Data;
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

        private static TAttribute GetAttribute<TAttribute>(Type itemType)
            where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(itemType).FirstOrDefault();
        }

        private static List<TAttribute> GetAttributes<TAttribute>(Type itemType)
            where TAttribute : Attribute
        {
            var allAttributes = itemType.GetCustomAttributes(true);
            if (allAttributes.Length == 0)
            {
                return null;
            }

            var matchingAttribute = allAttributes.OfType<TAttribute>().ToList();
            return matchingAttribute;
        }
    }
}
