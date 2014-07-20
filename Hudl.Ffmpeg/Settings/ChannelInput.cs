using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
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
