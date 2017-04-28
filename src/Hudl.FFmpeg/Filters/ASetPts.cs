using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Changes the PTS (presentation timestamp of the input frames)
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    [Filter(Name = "asetpts", MinInputs = 1, MaxInputs = 1)]
    public class ASetPts : IFilter
    {
        public const string ResetPtsExpression = "PTS-STARTPTS";
        public const string FormatPlaybackRateExpression = "{0}*PTS"; 

        public ASetPts() 
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
        [FilterParameter(Name = "expr")]
        public string Expression { get; set; }
    }
}
