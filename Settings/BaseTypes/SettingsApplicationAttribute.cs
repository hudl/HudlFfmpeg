using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
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
            any = 0,
            /// <summary>
            /// indicates that the setting can only be applied to an input stream
            /// </summary>
            input = 1, 
            /// <summary>
            /// indicates that the settig can only be applied to the output stream
            /// </summary>
            output = 2
        }

        public bool PreDeclaration { get; set; } 
        public SettingsResourceType ResourceType { get; set; } 
    }
}
