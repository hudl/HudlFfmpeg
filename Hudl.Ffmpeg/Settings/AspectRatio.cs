using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// set the video display aspect ratio specified by aspect.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class AspectRatio : BaseSetting
    {
        private const string SettingType = "-aspect";

        public AspectRatio()
            : base(SettingType)
        {
        }
        public AspectRatio(FfmpegRatio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; }

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
