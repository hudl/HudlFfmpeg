using System;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// data class that provides the data from the ISettings Attribute.  
    /// </summary>
    internal class SettingsApplicationData 
    {
        public bool PreDeclaration { get; set; } 

        public SettingsCollectionResourceTypes ResourceType { get; set; } 
    }
}
