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

        public Scale()
            : base(FilterType, FilterMaxInputs)
        {
            Dimensions = new Point(0, 0);
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

            Dimensions = new Point(x, y);
        }
        public Scale(string expression)
            : this()
        {
            Expression = expression;
        }

        public Point Dimensions { get; set; }
        public string Expression { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Expression))
            {
                return string.Concat(Type, "=", Expression);
            }

            if (Dimensions.X <= 0)
            {
                throw new InvalidOperationException("Dimensions.X must be greater than zero for scaling.");
            }
            if (Dimensions.Y <= 0)
            {
                throw new InvalidOperationException("Dimensions.Y must be greater than zero for scaling.");
            }

            return string.Concat(Type, "=w=", Dimensions.X, ":h=", Dimensions.Y, ":flags=lanczos");
        }
    }
}
