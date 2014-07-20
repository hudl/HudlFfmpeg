using System;
using System.Text;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// SetSar Filter, sets the Sample Aspect Ratio for the video resource.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    public class SetSar : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setsar";

        public SetSar()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public SetSar(FfmpegRatio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentException("Ratio cannot be null.", "ratio");
            }

            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; }

        public override void Validate()
        {
            if (Ratio == null)
            {
                throw new InvalidOperationException("Ratio cannot be null.");
            }
        }

        public override string ToString() 
        {
            var filterParameters = new StringBuilder(100);

            if (Ratio != null)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "sar", Ratio);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
