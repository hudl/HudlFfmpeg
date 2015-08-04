using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;
using System;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class VideoFrames : BaseSetting
    {
         private const string SettingType = "-frames:v";

         public VideoFrames(int framesCount)
            : base(SettingType)
        {
            if (framesCount <= 0)
            {
                throw new ArgumentNullException("framesCount");
            }

            FramesCount = framesCount;
        }

        public int FramesCount { get; set; }

        public override string ToString()
        {
            if (FramesCount <= 0)
            {
                throw new InvalidOperationException("FramesCount must be greater than zero for this setting.");
            }

            return string.Concat(Type, " ", FramesCount);
        }
    }
}