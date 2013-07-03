using System;
using System.Collections.Generic;
using System.Drawing; 
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Scale Filter, scales the output stream to match the filter settings.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    public class Scale : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "scale";

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

        public Scale()
            : base(FilterType, FilterMaxInputs)
        {
            Dimensions = new Point(0, 0);
        }
        public Scale(ScalePresetTypes preset)
            : this() 
        {
            if (!_scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Dimensions = _scalingPresets[preset];
        }
        public Scale(int x, int y)
            : this()
        {
            if (x <= 0)
            {
                throw new ArgumentException("X must be greater than zero for scaling.");
            }
            if (y <= 0)
            {
                throw new ArgumentException("Y must be greater than zero for scaling.");
            }

            Dimensions = new Point(x, y);
        }

        public Point Dimensions { get; set; }

        public override string ToString()
        {
            if (Dimensions.X <= 0)
            {
                throw new InvalidOperationException("Dimensions.X must be greater than zero for scaling.");
            }
            if (Dimensions.Y <= 0)
            {
                throw new InvalidOperationException("Dimensions.Y must be greater than zero for scaling.");
            }

            return string.Concat(Type, "=w=", Dimensions.X, ":h=", Dimensions.Y);
        }
    }
}
