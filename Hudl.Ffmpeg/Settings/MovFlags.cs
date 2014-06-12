using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// MovFlags is used to set AVOptions that tell ffmpeg how to fragment the file. 
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class MovFlags : BaseSetting
    {
        private const string SettingType = "-movflags";
 
        /// <summary>
        /// run a second pass moving the index (moov atom) to the beginning of the file
        /// </summary>
        public const string EnableFastStart = "+faststart"; 

        public MovFlags(string flags)
            : base(SettingType)
        {
            Flags = flags;
        }
    
        public string Flags { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Flags))
            {
                throw new InvalidOperationException("Flags cannot be null or empty.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Flags);
        }
    }
}
