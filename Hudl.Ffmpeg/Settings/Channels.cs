using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Channels is used to set the number of output channels in a video stream. 
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Channels : BaseSetting
    {
        private const string SettingType = "-ac";
        
        public Channels(int numberOfChannels)
            : base(SettingType)
        {
            NumberOfChannels = numberOfChannels;
        }

        public int NumberOfChannels { get; set; }

        public override void Validate()
        {
            if (NumberOfChannels <= 0)
            {
                throw new InvalidOperationException("NumberOfChannels must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", NumberOfChannels);
        }
    }
}
