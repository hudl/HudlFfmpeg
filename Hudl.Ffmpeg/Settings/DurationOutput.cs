using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// When used as an output option (before an output filename), stop writing the output after its duration reaches duration.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IAudio))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class DurationOutput : BaseDuration
    {
        public DurationOutput(TimeSpan length)
            : base(length)
        {
        }
        public DurationOutput(double seconds)
            : base(seconds)
        {
        }
    }
}
