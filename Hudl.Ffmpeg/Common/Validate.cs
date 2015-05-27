using System;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.Interfaces;
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
        /// returns a boolean indicating if <cref name="TObject"/> is applicable to <cref name="TRestrictedTo"/> 
        /// </summary>
        /// <typeparam name="TObject">the type in question to be applied to</typeparam>
        /// <typeparam name="TRestrictedTo">the type in question that is required</typeparam>
        public static bool AppliesTo<TObject, TRestrictedTo>()
            where TRestrictedTo : IStream
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
            var matchingAttributes = AttributeRetrieval.GetAttributes<ForStreamAttribute>(objectType);
            if (matchingAttributes.Count == 0)
            {
                return false;
            }

            return matchingAttributes.Any(attribute => (attribute.Type == restrictedType ||
                                                        attribute.Type.IsAssignableFrom(restrictedType)));
        }

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
            return type == item.GetResourceType();
        }

    }
}
