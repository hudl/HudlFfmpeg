using System;
using Hudl.FFmpeg.Common;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    /// <summary>
    /// attribute that decorates an ISettings class, it is responsible for 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class SettingsApplicationAttribute : Attribute
    {
        public SettingsApplicationAttribute()
        {
            Data = new SettingsApplicationData();
        }

        /// <summary>
        /// Pre declaration indicates that the setting must go before the setting declaration
        /// </summary>
        public bool PreDeclaration
        {
            get { return Data.PreDeclaration; } 
            set { Data.PreDeclaration = value; }
        } 

        /// <summary>
        /// Resource Type indicates what type of resource the setting applies to.
        /// </summary>
        public SettingsCollectionResourceType ResourceType
        {
            get { return Data.ResourceType; }
            set { Data.ResourceType = value; }
        }

        /// <summary>
        /// Multiple allowed indicates that the setting type does have a limit on number
        /// </summary>
        public bool MultipleAllowed
        {
            get { return Data.MultipleAllowed; }
            set { Data.MultipleAllowed = value; }
        }

        /// <summary>
        /// internal accessor to get the data while parsing the attributes.
        /// </summary>
        internal SettingsApplicationData Data { get; set; }
    }
}
