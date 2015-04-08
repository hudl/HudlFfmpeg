using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set bitstream filters for matching video streams. bitstream_filters is a comma-separated list of bitstream filters. 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitStreamFilterVideo : BaseBitStreamFilter
    {
        private const string Suffix = ":v";

        public BitStreamFilterVideo(string setting)
            : base(Suffix, setting)
        {
        }
    }
}
