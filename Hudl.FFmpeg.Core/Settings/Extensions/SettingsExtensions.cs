using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.Utility
{
    public static class SettingsUtility
    {
        public static bool IsPreSetting(this ISetting setting)
        {
            var settingsAttribute = GetSettingsAttribute(setting);

            return settingsAttribute.IsPreDeclaration;
        }

        public static bool IsPostSetting(this ISetting setting)
        {
            var settingsAttribute = GetSettingsAttribute(setting);

            return settingsAttribute.IsPreDeclaration;
        }

        public static bool IsMultipleAllowed(this ISetting setting)
        {
            var settingsAttribute = GetSettingsAttribute(setting);

            return settingsAttribute.IsMultipleAllowed;
        }

        public static bool IsParameterless(this ISetting setting)
        {
            var settingsAttribute = GetSettingsAttribute(setting);

            return settingsAttribute.IsMultipleAllowed;
        }

        public static SettingsCollectionResourceType GetResourceType(this ISetting setting)
        {
            var settingsAttribute = GetSettingsAttribute(setting);

            return settingsAttribute.ResourceType;
        }

        private static SettingAttribute GetSettingsAttribute(ISetting setting)
        {
            var settingsAttribute = AttributeRetrieval.GetAttribute<SettingAttribute>(setting.GetType());
            if (settingsAttribute == null)
            {
                throw new InvalidOperationException(string.Format("ISetting \"{0}\", does not include the SettingAttribute.", setting.GetType().Name));
            }

            return settingsAttribute;
        }

    }
}
