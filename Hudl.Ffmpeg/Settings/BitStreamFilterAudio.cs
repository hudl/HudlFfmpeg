using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set bitstream filters for matching audio streams. bitstream_filters is a comma-separated list of bitstream filters. 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitStreamFilterAudio : BaseBitStreamFilter
    {
        private const string Suffix = ":a";

        public const string ConvertAdtsToAsc = "aac_adtstoasc";

        public BitStreamFilterAudio(string setting)
            : base(Suffix, setting)
        {
        }
    }
}
