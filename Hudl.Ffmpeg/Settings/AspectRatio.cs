using System;
using System.Drawing;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
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
               
        public override string ToString()  
        {
            if (Ratio == null)
            {
                throw new InvalidOperationException("Ratio cannot be null.");
            }

            return string.Concat(Type, " ", Ratio.ToRatio());
        }
    }
}
