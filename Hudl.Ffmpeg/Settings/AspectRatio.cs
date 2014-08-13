using System;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set the video display aspect ratio specified by aspect.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class AspectRatio : BaseSetting
    {
        private const string SettingType = "-aspect";

        public AspectRatio()
            : base(SettingType)
        {
        }
        public AspectRatio(FFmpegRatio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        public FFmpegRatio Ratio { get; set; }

        public override void Validate()
        {
            if (Ratio == null)
            {
                throw new InvalidOperationException("Ratio cannot be null.");
            }
        }

        public override string ToString()  
        {
            return string.Concat(Type, " ", Ratio.ToRatio());
        }
    }
}
