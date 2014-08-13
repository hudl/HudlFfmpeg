using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set the video target encoding profile
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
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
