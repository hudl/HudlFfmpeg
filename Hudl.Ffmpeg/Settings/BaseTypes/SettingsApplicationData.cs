using System;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    /// <summary>
    /// data class that provides the data from the ISettings Attribute.  
    /// </summary>
    internal class SettingsApplicationData 
    {
        public bool PreDeclaration { get; set; }

        public bool MultipleAllowed { get; set; }

        public SettingsCollectionResourceType ResourceType { get; set; } 
    }
}
