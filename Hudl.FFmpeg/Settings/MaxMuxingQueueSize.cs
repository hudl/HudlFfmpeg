using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
namespace Hudl.FFmpeg.Settings
{

    /// <summary>
    /// specifies a max muxing queue size.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "max_muxing_queue_size")]
    public class MaxMuxingQueueSize : ISetting
    {
        public MaxMuxingQueueSize(double queueSize)
        {
            QueueSize = queueSize;
        }

        [SettingParameter]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double QueueSize { get; set; }
    }
}
