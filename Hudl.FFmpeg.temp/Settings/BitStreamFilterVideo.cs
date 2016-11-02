using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set bitstream filters for matching video streams. bitstream_filters is a comma-separated list of bitstream filters. 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "bsf:v")]
    public class BitStreamFilterVideo : BaseBitStreamFilter
    {
        public BitStreamFilterVideo(string setting)
            : base( setting)
        {
        }
    }
}
