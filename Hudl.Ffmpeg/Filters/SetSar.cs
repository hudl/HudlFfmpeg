using System;
using System.Text;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common.DataTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
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
        public SetSar(Ratio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentException("Ratio cannot be null.", "ratio");
            }

            Ratio = ratio;
        }

        public Ratio Ratio { get; set; }

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
                FilterUtility.ConcatenateParameter(filterParameters, "sar", Ratio.ToFractionalString());
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
