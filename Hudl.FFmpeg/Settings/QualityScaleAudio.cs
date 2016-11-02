using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the quality for a video 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "q:v")]
    public class QualityScaleVideo : BaseQualityScale
    {
        public QualityScaleVideo(int scale)
            : base(scale)
        { 
        }
    }
}
