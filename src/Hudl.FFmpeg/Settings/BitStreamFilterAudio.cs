using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set bitstream filters for matching audio streams. bitstream_filters is a comma-separated list of bitstream filters. 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "bsf:a")]
    public class BitStreamFilterAudio : BaseBitStreamFilter
    {
        public const string ConvertAdtsToAsc = "aac_adtstoasc";

        public BitStreamFilterAudio(string setting)
            : base(setting)
        {
        }
    }
}
