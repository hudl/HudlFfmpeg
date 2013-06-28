using System;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// attribute that decorates an ISettings class, it is responsible for 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class SettingsApplicationAttribute : Attribute
    {
        /// <summary>
        /// an enumeration describing how the setting applies to the command
        /// </summary>
        public enum SettingsResourceType 
        {
            /// <summary>
            /// indicates that the setting can be applied to input and output type
            /// </summary>
            Any = 0,
            /// <summary>
            /// indicates that the setting can only be applied to an input stream
            /// </summary>
            Input = 1, 
            /// <summary>
            /// indicates that the settig can only be applied to the output stream
            /// </summary>
            Output = 2
        }

        public bool PreDeclaration { get; set; } 

        public SettingsResourceType ResourceType { get; set; } 
    }
}
