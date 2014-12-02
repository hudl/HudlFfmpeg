using System;
using System.Text;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Changes the PTS (presentation timestamp of the input frames)
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    public class ASetPts : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "asetpts";
        public const string ResetPtsExpression = "PTS-STARTPTS";
        public const string FormatPlaybackRateExpression = "{0}*PTS"; 

        public ASetPts() 
            : base(FilterType, FilterMaxInputs)
        {
        }
        public ASetPts(string expression)
            : this()
        {
            Expression = expression;
        }
        public ASetPts(SetPtsExpressionType expressionType)
            : this()
        {
            if (expressionType == SetPtsExpressionType.ResetTimestamp)
            {
                Expression = ResetPtsExpression;
            }
        }

        /// <summary>
        /// the setpts expression details can be found at http://ffmpeg.org/ffmpeg-all.html#setpts_002c-asetpts
        /// </summary>
        public string Expression { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with a set PTS filter");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (!string.IsNullOrWhiteSpace(Expression))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "expr", Expression);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
