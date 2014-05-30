using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// MovFlags sets the LibAV Encoder options to enable features and options in the video
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class MovFlags : BaseSetting
    {
        private const string SettingType = "-movflags";
        public const string EnableFastStart = "+faststart"; 

        public MovFlags(string flags)
            : base(SettingType)
        {
            Flags = flags;
        }
    
        public string Flags { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Flags))
            {
                throw new InvalidOperationException("MovFlags cannot be null or empty.");
            }

            return string.Concat(Type, " ", Flags);
        }
    }
}
