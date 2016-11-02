using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the quality for a audio 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "q:a")]
    public class QualityScaleAudio : BaseQualityScale
    {
        public QualityScaleAudio(int scale)
            : base(scale)
        { 
        }
    }
}
