using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// MovFlags is used to set AVOptions that tell ffmpeg how to fragment the file. 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
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
