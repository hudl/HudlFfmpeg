using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// when used as an input option (before -i), limit the duration of data read from the input file.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)]
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
