using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set the video target encoding profile
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "profile:v")]
    public class ProfileVideo : BaseProfile
    {
        public ProfileVideo(string codec)
            : base(codec)
        {
        }
        public ProfileVideo(VideoProfileType profile)
            : base(Formats.Library(profile.ToString()))
        {
        }
    }
}
