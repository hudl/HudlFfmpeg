using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// when used as an input option (before -i), limit the duration of data read from the input file.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "t", IsPreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class DurationInput : BaseDuration
    {
        public DurationInput(TimeSpan length)
            : base(length)
        {
        }
        public DurationInput(double seconds)
            : base(seconds)
        {
        }
    }
}
