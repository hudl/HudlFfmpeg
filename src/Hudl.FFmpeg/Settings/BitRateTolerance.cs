using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the bitrate tolerance for the output stream 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "bt")]
    public class BitRateTolerance : BaseBitRate
    {
        public BitRateTolerance(int rate)
            : base(rate)
        {
        }
    }
}
