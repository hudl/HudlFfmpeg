using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class ProfileVideo : BaseProfile
    {
        private const string Suffix = ":v";

        public ProfileVideo(string codec)
            : base(Suffix, codec)
        {
        }
        public ProfileVideo(VideoProfileType profile)
            : base(Suffix, Formats.Library(profile.ToString()))
        {
        }
    }
}
