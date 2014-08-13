using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Set the number of audio channels. For input streams this option only makes sense for audio grabbing devices and raw demuxers and is mapped to the corresponding demuxer options.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)]
    public class ChannelInput : BaseChannel
    {
        public ChannelInput(int numberOfChannels)
            : base(numberOfChannels)
        {
        }
    }
}
