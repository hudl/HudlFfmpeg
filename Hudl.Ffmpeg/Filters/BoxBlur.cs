using System;
using System.Text;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Video filter that applies a boxblur algorithm to the input video.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    public class BoxBlur : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "boxblur";

         public BoxBlur()
            : base(FilterType, FilterMaxInputs)
        {
        }

        public BoxBlur(string expression)
            : this()
        {
            Expression = expression; 
        }

        public string Expression { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression of the BoxBlur cannot be null or whitespace.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (!string.IsNullOrWhiteSpace(Expression))
            {
                FilterUtility.ConcatenateParameter(filterParameters, Expression); 
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
