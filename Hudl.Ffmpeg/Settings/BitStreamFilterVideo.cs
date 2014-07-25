using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
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
