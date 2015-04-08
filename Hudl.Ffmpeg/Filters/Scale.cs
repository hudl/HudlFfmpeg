using System;
using System.Drawing;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Scale Filter, scales the output stream to match the filter settings.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
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

        public Size? Dimensions { get; set; }

        public int? Interlacing { get; set; }

        public string Flags { get; set; }

        public VideoScalingColorMatrixType InColorMatrix { get; set; }

        public VideoScalingColorMatrixType OutColorMatrix { get; set; }

        public VideoScalingRangeType InRange { get; set; }

        public VideoScalingRangeType OutRange { get; set; }

        public VideoScalingAspectRatioType ForceAspectRatio { get; set; }

        public override void Validate()
        {
            if (Dimensions.HasValue && Dimensions.Value.Width <= 0)
            {
                throw new InvalidOperationException("Dimensions.X must be greater than zero for scaling.");
            }
            if (Dimensions.HasValue && Dimensions.Value.Height <= 0)
            {
                throw new InvalidOperationException("Dimensions.Y must be greater than zero for scaling.");
            }
            if (Interlacing.HasValue && (Interlacing >= 1 || Interlacing <= -1))
            {
                throw new InvalidOperationException("Interlacing flag must be a value of 1, 0, or -1 for scaling.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (Dimensions.HasValue && Dimensions.Value.Width > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "w", Dimensions.GetValueOrDefault().Width);
            }
            if (Dimensions.HasValue && Dimensions.Value.Height > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "h", Dimensions.GetValueOrDefault().Height);
            }
            if (Interlacing.HasValue && Interlacing != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "interl", Interlacing); 
            }
            if (!string.IsNullOrWhiteSpace(Flags))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "flags", Flags); 
            }
            if (InColorMatrix != VideoScalingColorMatrixType.Auto)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "in_color_matrix", Formats.EnumValue(InColorMatrix));
            }
            if (OutColorMatrix != VideoScalingColorMatrixType.Auto)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "out_color_matrix", Formats.EnumValue(OutColorMatrix));
            }
            if (InRange != VideoScalingRangeType.Auto)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "in_range", Formats.EnumValue(InRange, true));
            }
            if (OutRange != VideoScalingRangeType.Auto)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "out_range", Formats.EnumValue(OutRange, true));
            }
            if (ForceAspectRatio != VideoScalingAspectRatioType.Disable)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "force_original_aspect_ratio", Formats.EnumValue(ForceAspectRatio));
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
