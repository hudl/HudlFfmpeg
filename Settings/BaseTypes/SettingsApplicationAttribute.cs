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
        public bool PreDeclaration { get; set; } 

        public SettingsCollectionResourceTypes ResourceType { get; set; } 
    }
}
