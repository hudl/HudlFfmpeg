using System;
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

        public Scale()
            : base(FilterType, FilterMaxInputs)
        {
            Dimensions = new Size(0, 0);
        }
        public Scale(ScalePresetType preset)
            : this()
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Dimensions = scalingPresets[preset];
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

            Dimensions = new Size(x, y);
        }

        public Size Dimensions { get; set; }

        public override void Validate()
        {
            if (Dimensions.Width <= 0)
            {
                throw new InvalidOperationException("Dimensions.X must be greater than zero for scaling.");
            }
            if (Dimensions.Height <= 0)
            {
                throw new InvalidOperationException("Dimensions.Y must be greater than zero for scaling.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, "=w=", Dimensions.Width, ":h=", Dimensions.Height, ":flags=lanczos");
        }
    }
}
