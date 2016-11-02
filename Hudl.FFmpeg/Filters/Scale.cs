using System;
using System.Drawing;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Scale Filter, scales the output stream to match the filter settings.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "scale", MinInputs = 1, MaxInputs = 1)]
    public class Scale : IFilter
    {
        public Scale()
        {
        }
        public Scale(ScalePresetType preset)
            : this()
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Width = scalingPresets[preset].Width;
            Height = scalingPresets[preset].Height;
        }
        public Scale(int width, int height)
            : this()
        {
            if (width <= 0)
            {
                throw new ArgumentException("Width must be greater than zero for scaling.");
            }
            if (height <= 0)
            {
                throw new ArgumentException("Height must be greater than zero for scaling.");
            }

            Width = width;
            Height = height;
        }

        [FilterParameter(Name = "w")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? Width { get; set; }

        [FilterParameter(Name = "h")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? Height { get; set; }

        [FilterParameter(Name = "interl")]
        [Validate(LogicalOperators.IsOneOf, -1, 0, 1)]
        public int? Interlacing { get; set; }

        [FilterParameter(Name = "flags")]
        public string Flags { get; set; }

        [FilterParameter(Name = "in_color_matrix", Default = VideoScalingColorMatrixType.Auto, Formatter = typeof(EnumParameterFormatter))]
        public VideoScalingColorMatrixType? InColorMatrix { get; set; }

        [FilterParameter(Name = "out_color_matrix", Default = VideoScalingColorMatrixType.Auto, Formatter = typeof(EnumParameterFormatter))]
        public VideoScalingColorMatrixType? OutColorMatrix { get; set; }

        [FilterParameter(Name = "in_range", Default = VideoScalingRangeType.Auto, Formatter = typeof(EnumParameterSlashFormatter))]
        public VideoScalingRangeType? InRange { get; set; }

        [FilterParameter(Name = "out_range", Default = VideoScalingRangeType.Auto, Formatter = typeof(EnumParameterSlashFormatter))]
        public VideoScalingRangeType? OutRange { get; set; }

        [FilterParameter(Name = "force_original_aspect_ratio", Default = VideoScalingAspectRatioType.Disable, Formatter = typeof(EnumParameterFormatter))]
        public VideoScalingAspectRatioType? ForceAspectRatio { get; set; }
    }
}
