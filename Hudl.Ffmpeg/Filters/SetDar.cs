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
    /// SetDar Filter, sets the Dynamic Aspect Ratio for the video resource.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    public class SetDar : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setdar";

        public SetDar()
            : base(FilterType, FilterMaxInputs)
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
                FilterUtility.ConcatenateParameter(filterParameters, "dar", Ratio.ToFractionalString()); 
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
