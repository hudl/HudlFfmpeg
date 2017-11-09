using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using System.Collections.Generic;
using System.Drawing;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Applies Zoom & Pan effect.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name ="zoompan", MinInputs = 1, MaxInputs = 1)]
    public class ZoomPan : IFilter
    {
        public ZoomPan()
        {
        }

        public ZoomPan(string zoom, string x, string y, string d, double? fps, Size? size)
        {
            Zoom = zoom;
            X = x;
            Y = y;
            D = d;
            S = size;
            Fps = fps;
        }

        public ZoomPan(string zoom, string x, string y, string d, double? fps, ScalePresetType? preset)
            : this(zoom, x, y, d, fps, (Size?)null)
        {
            Size sizeValue = default(Size); 
            if (preset.HasValue && Helpers.ScalingPresets.TryGetValue(preset.Value, out sizeValue))
            {
                S = sizeValue;
            }
        }

        [FilterParameter(Name = "zoom", Formatter = typeof(SingleQuoteExpressionFormatter))]
        public string Zoom { get; set; }

        [FilterParameter(Name = "x", Formatter = typeof(SingleQuoteExpressionFormatter))]
        public string X { get; set; }

        [FilterParameter(Name = "y", Formatter = typeof(SingleQuoteExpressionFormatter))]
        public string Y { get; set; }

        [FilterParameter(Name = "d")]
        public string D { get; set; }

        [FilterParameter(Name = "s", Formatter = typeof(SizeFormatter))]
        public Size? S { get; set; }

        [FilterParameter(Name = "fps")]
        public double? Fps { get; set; }
    }
}
