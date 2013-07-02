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
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Output)]
    public class Dimensions : BaseSetting
    {
        private const string SettingType = "-s";

        private readonly Dictionary<ScalePresetTypes, Point> _scalingPresets = new Dictionary<ScalePresetTypes, Point>
        {
            { ScalePresetTypes.Svga, new Point(800, 600) }, 
            { ScalePresetTypes.Xga, new Point(1024, 768) }, 
            { ScalePresetTypes.Ega, new Point(640, 350) }, 
            { ScalePresetTypes.Sd240, new Point(432, 240) }, 
            { ScalePresetTypes.Sd360, new Point(640, 360) }, 
            { ScalePresetTypes.Hd480, new Point(852, 480) }, 
            { ScalePresetTypes.Hd720, new Point(1280, 720) },
            { ScalePresetTypes.Hd1080, new Point(1920, 1080) }
        };

        public Dimensions()
            : base(SettingType)
        {
            Size = new Point(1, 1);
        }
        public Dimensions(ScalePresetTypes preset)
            : this()
        {
            if (!_scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Size = _scalingPresets[preset];
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
                throw new ArgumentException("Dimensions size cannot be null.");
            }
            if (Size.X <= 0)
            {
                throw new ArgumentException("Dimensions width must be greater than zero.");
            }
            if (Size.Y <= 0)
            {
                throw new ArgumentException("Dimensions height must be greater than zero.");
            }

            return string.Concat(Type, " ", Size.X, "x", Size.Y);
        }
    }
}
