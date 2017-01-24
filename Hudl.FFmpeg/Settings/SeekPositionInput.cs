using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Seek to should be used when StartAt cannot be used, FFmpeg will process the video up to the timestamp provided, but discard it. 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "ss", IsPreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)]
    public class SeekPositionInput : BaseSeekPosition
    {
        public SeekPositionInput(TimeSpan length)
            : base (length)
        {
        }
        public SeekPositionInput(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }
    }
}
