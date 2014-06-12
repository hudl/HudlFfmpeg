using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Changes the PTS (presentation timestamp of the input frames)
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class SetPts : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setpts";
        private const string ResetPtsExpression = "PTS-STARTPTS";
        public const string FormatPlaybackRateExpression = "{0}*PTS"; 

        public SetPts() 
            : base(FilterType, FilterMaxInputs)
        {
        }
        public SetPts(string expression)
            : this()
        {
            Expression = expression;
        }
        public SetPts(bool resetTimestamp)
            : this()
        {
            if (resetTimestamp)
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
            return string.Concat(Type, "=", Expression);
        }
    }
}
