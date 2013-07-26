using System;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
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
            : this(ResetPtsExpression)
        {
        }

        /// <summary>
        /// the setpts expression details can be found at http://ffmpeg.org/ffmpeg-all.html#setpts_002c-asetpts
        /// </summary>
        public string Expression { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with a set PTS filter");
            }

            return string.Concat(Type, "=", Expression);
        }
    }
}
