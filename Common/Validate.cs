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
    internal class Validate
    {
        /// <summary>
        /// returns a boolean indicating if <cref name="TObject"/> is applicable to <cref name="TRestrictedTo"/> 
        /// </summary>
        /// <typeparam name="TObject">the type in question to be applied to</typeparam>
        /// <typeparam name="TRestrictedTo">the type in question that is required</typeparam>
        public static bool AppliesTo<TObject, TRestrictedTo>()
            where TRestrictedTo : IResource
        {
            var typeAAttributes = new List<CustomAttributeData>(typeof(TObject).CustomAttributes); 
            var typeAAppliesToAttributes = typeAAttributes.FindAll(a => a.AttributeType == typeof(AppliesToResourceAttribute));
            if (typeAAppliesToAttributes.Count == 0)
            {
                return false;
            }
            var typeAAppliesToAttributeType = typeAAppliesToAttributes.Find(a => a.NamedArguments.Any(namedArg => namedArg.MemberName == "Type" && namedArg.TypedValue.Value is TRestrictedTo));
            return (typeAAppliesToAttributeType != null); 
        }

        public static bool IsSettingFor<TSetting>(SettingsCollectionResourceTypes type)
            where TSetting : ISetting
        {
            var typeAAttributes = new List<CustomAttributeData>(typeof (TSetting).CustomAttributes);
            var typeASettingsAttributes = typeAAttributes.FirstOrDefault(a => a.AttributeType == typeof(SettingsApplicationAttribute));
            if (typeASettingsAttributes == null || typeASettingsAttributes.NamedArguments == null)
            {
                return false;
            }
            var resourceTypeArgument = typeASettingsAttributes.NamedArguments
                                                              .FirstOrDefault(a => a.MemberName == "ResourceType");
            var resourceTypeValue = (SettingsCollectionResourceTypes)resourceTypeArgument.TypedValue.Value;
            return (type == resourceTypeValue);
        }

        public static SettingsApplicationData GetSettingData<TSetting>()
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

        public static SettingsApplicationData GetSettingData<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            return GetSettingData<TSetting>();
        }

        public static Dictionary<Type, SettingsApplicationData> GetSettingCollectionData(SettingsCollection collection)
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
