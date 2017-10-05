using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Set frame rate (Hz value, fraction or abbreviation).
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "r", ResourceType = SettingsCollectionResourceType.Input)]
    public class FrameRateInput : BaseFrameRate
    {
        public FrameRateInput()
            : base()
        {
        }
        public FrameRateInput(double rate)
            : base(rate)
        {
        }
    }
}
