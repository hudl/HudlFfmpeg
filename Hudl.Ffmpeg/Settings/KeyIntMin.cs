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
    public class KeyIntMin : BaseSetting
    {
        private const string SettingType = "-keyint_min";

        public KeyIntMin(int size)
            : base(SettingType)
        {
            Size = size;
        }

        public double Size { get; set; }

        public override string ToString()
        {
            if (Size <= 0)
            {
                throw new InvalidOperationException("KeyIntMin size must be greater than zero.");
            }

            return string.Concat(Type, " ", Size);
        }
    }
}
