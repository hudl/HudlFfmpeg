using System;

namespace Hudl.FFmpeg.Settings.Attributes
{
    /// <summary>
    /// attribute that decorates an ISettings class, it is responsible for 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class SettingValueAttribute : Attribute
    {
        public SettingValueAttribute()
        {
        }

        public Type Formatter{ get; set; }
    }
}
