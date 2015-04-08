using System;
using System.Drawing;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Filter that applies padding to input video 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    public class Pad : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "pad";

        public const string ExprConvertTo169Aspect = "ih*16/9:ih:(ow-iw)/2:(oh-ih)/2"; 

        public Pad() 
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Pad(Size? toDimensions, Point? atPosition)
            : this()
        {
            Dimensions = toDimensions;
            Offset = atPosition;
        }
        public Pad(string expression)
            : this()
        {
            Expression = expression;
        }

        public string Expression { get; set; }

        public Size? Dimensions { get; set; }

        public Point? Offset { get; set; }

        public string Color { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                if (Offset.HasValue && Offset.Value.IsEmpty)
                {
                    throw new InvalidOperationException("Offset point cannot be empty.");
                }
                if (Dimensions.HasValue && Dimensions.Value.IsEmpty)
                {
                    throw new InvalidOperationException("Dimensions cannot be empty.");
                }
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100); 

            if (!string.IsNullOrWhiteSpace(Expression))
            {
                FilterUtility.ConcatenateParameter(filterParameters, Expression);

                return FilterUtility.JoinTypeAndParameters(this, filterParameters); 
            }

            if (Dimensions.HasValue && Dimensions.Value.Width != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "w", Dimensions.Value.Width);
            }
            if (Dimensions.HasValue && Dimensions.Value.Height != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "h", Dimensions.Value.Height);
            }
            if (Offset.HasValue && Offset.Value.X != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "x", Offset.Value.X);
            }
            if (Offset.HasValue && Offset.Value.Y != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "y", Offset.Value.Y);
            }
            if (!string.IsNullOrWhiteSpace(Color))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "color", Color);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters); 
        }
    }
}
