using System;
using System.Collections.Generic;
using System.Drawing;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class SceneChangeThreshold : BaseSetting
    {
        private const string SettingType = "-sc_threshold";

        public SceneChangeThreshold(int size)
            : base(SettingType)
        {
            Size = size;
        }

        public int Size { get; set; }

        public override string ToString()
        {
            if (Size < 0)
            {
                throw new InvalidOperationException("SceneChangeThreshold size must be greater than or equal to zero.");
            }
            if (Size > 100)
            {
                throw new InvalidOperationException("SceneChangeThreshold size must be less than or equal to 100.");
            }

            return string.Concat(Type, " ", Size);
        }
    }
}
