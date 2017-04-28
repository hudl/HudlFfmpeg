using System;
using System.Linq;
using System.Reflection;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Settings.Utility;

namespace Hudl.FFmpeg.Common
{
    /// <summary>
    /// helper class that helps with validation of objects in a ffmpeg project
    /// </summary>
    internal class Validate
    {
        /// <summary>
        /// returns a boolean indicating if <cref name="objectType"/> is applicable to <cref name="restrictedType"/> 
        /// </summary>
        public static bool ContainsStream(Type objectType, Type streamType)
        {
            var matchingAttributes = AttributeRetrieval.GetAttributes<ContainsStreamAttribute>(objectType);
            if (matchingAttributes.Count == 0)
            {
                return false;
            }

            return matchingAttributes.Any(attribute => (attribute.Type == streamType ||
                                                        attribute.Type.IsAssignableFrom(streamType)));
        }

        public static bool IsSettingFor<TSetting>(TSetting item, SettingsCollectionResourceType type)
            where TSetting : ISetting
        {
            return item.GetResourceType() == SettingsCollectionResourceType.Any ||
                type == item.GetResourceType();
        }

    }
}
