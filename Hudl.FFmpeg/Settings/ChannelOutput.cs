using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Set the number of audio channels. For output streams it is set by default to the number of input audio channels.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "ac")]
    public class ChannelOutput : BaseChannel
    {
        public ChannelOutput(int numberOfChannels)
            : base(numberOfChannels)
        {
        }
    }
}
