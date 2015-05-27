using System;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Settings.Attributes
{
    /// <summary>
    /// attribute that decorates an ISettings class, it is responsible for 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class SettingAttribute : Attribute
    {
        public SettingAttribute()
        {
        }

        public string Name { get; set; }

        /// <summary>
        /// Pre declaration indicates that the setting must go before the setting declaration
        /// </summary>
        public bool IsPreDeclaration { get; set; }

        /// <summary>
        /// Multiple allowed indicates that the setting type does have a limit on number
        /// </summary>
        public bool IsMultipleAllowed { get; set; }

        /// <summary>
        /// Resource Type indicates what type of resource the setting applies to.
        /// </summary>
        public SettingsCollectionResourceType ResourceType { get; set; }
    }
}
