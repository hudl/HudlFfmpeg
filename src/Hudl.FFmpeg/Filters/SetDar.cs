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
    /// SetDar Filter, sets the Dynamic Aspect Ratio for the video resource.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "setdar", MinInputs = 1, MaxInputs = 1)]
    public class SetDar : IFilter
    {
        public SetDar()
        {
        }
        public SetDar(Ratio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        [FilterParameter(Name = "dar", Formatter = typeof(RatioFractionalStringFormatter))]
        public Ratio Ratio { get; set; }
    }
}
