using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Set the number of audio channels. For output streams it is set by default to the number of input audio channels.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class ChannelOutput : BaseChannel
    {
        public ChannelOutput(int numberOfChannels)
            : base(numberOfChannels)
        {
        }
    }
}
