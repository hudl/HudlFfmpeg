using System;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Settings.BaseTypes
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

        public bool PreDeclaration
        {
            get { return Data.PreDeclaration; } 
            set { Data.PreDeclaration = value; }
        } 

        public SettingsCollectionResourceTypes ResourceType
        {
            get { return Data.ResourceType; }
            set { Data.ResourceType = value; }
        }

        internal SettingsApplicationData Data { get; set; }
    }
}
