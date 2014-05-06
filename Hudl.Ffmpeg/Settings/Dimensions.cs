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
    public class Dimensions : BaseSetting
    {
        private const string SettingType = "-s";

        public Dimensions()
            : base(SettingType)
        {
            Size = new Point(1, 1);
        }
        public Dimensions(ScalePresetType preset)
            : this()
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Size = scalingPresets[preset];
        }
        public Dimensions(int x, int y)
            : this()
        {
            if (x <= 0)
            {
                throw new ArgumentException("Dimensions X must be greater than zero.");
            }
            if (y <= 0)
            {
                throw new ArgumentException("Dimensions Y must be greater than zero.");
            }

            Size = new Point(x, y);
        }

        public Point Size { get; set; }

        public override string ToString()
        {
            if (Size == null)
            {
                throw new InvalidOperationException("Dimensions size cannot be null.");
            }
            if (Size.X <= 0)
            {
                throw new InvalidOperationException("Dimensions width must be greater than zero.");
            }
            if (Size.Y <= 0)
            {
                throw new InvalidOperationException("Dimensions height must be greater than zero.");
            }

            return string.Concat(Type, " ", Size.X, "x", Size.Y);
        }
    }
}
