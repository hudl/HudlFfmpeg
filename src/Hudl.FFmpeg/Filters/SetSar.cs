using System;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// SetSar Filter, sets the Sample Aspect Ratio for the video resource.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "setsar", MinInputs = 1, MaxInputs = 1)]
    public class SetSar : IFilter
    {
        public SetSar()
        {
        }
        public SetSar(Ratio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentException("Ratio cannot be null.", "ratio");
            }

            Ratio = ratio;
        }

        [FilterParameter(Name = "sar", Formatter = typeof(RatioFractionalStringFormatter))]
        public Ratio Ratio { get; set; }
    }
}
