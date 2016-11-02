using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    ///     This option sets the maximum number of queued packets when reading from the file or device. With low latency / high
    ///     rate live streams, packets may be discarded if they are not read in a timely manner; raising this value can avoid
    ///     it.
    /// </summary>
    [ForStream(Type = typeof (VideoStream))]
    [ForStream(Type = typeof (AudioStream))]
    [Setting(Name = "thread_queue_size", IsPreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class ThreadQueueSize : ISetting
    {
        public ThreadQueueSize(int queueSize)
        {
            QueueSize = queueSize;
        }

        [SettingParameter]
        public int QueueSize { get; set; }
    }
}