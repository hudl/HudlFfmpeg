using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// SetDar Filter, sets the Dynamic Aspect Ratio for the video resource.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class SetDar : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setdar";

        public SetDar(FfmpegRatio ratio)
            : base(FilterType, FilterMaxInputs)
        {
            if (ratio == null)
            {
                throw new ArgumentException("Ratio cannot be null.", "ratio");
            }

            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; } 
               
        public override string ToString() 
        {
            if (Ratio == null)
            {
                throw new ArgumentException("Ratio cannot be null.");
            }

            return string.Concat(Type, "=dar=", Ratio);
        }
    }
}
