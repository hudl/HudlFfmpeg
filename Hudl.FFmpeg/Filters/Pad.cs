using System.Drawing;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Filter that applies padding to input video 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "pad", MinInputs = 1, MaxInputs = 1)]
    public class Pad : IFilter
    {
        public const string ExprConvertTo169Aspect = "ih*16/9:ih:(ow-iw)/2:(oh-ih)/2"; 

        public Pad() 
        {
        }
        public Pad(Size? toDimensions, Point? atPosition)
            : this()
        {
            Width = toDimensions.HasValue ? toDimensions.Value.Width : (int?)null;
            Height = toDimensions.HasValue ? toDimensions.Value.Height : (int?)null;
            X = atPosition.HasValue ? atPosition.Value.X : (int?)null;
            Y = atPosition.HasValue ? atPosition.Value.Y : (int?)null;
        }
        public Pad(string expression)
            : this()
        {
            Expression = expression;
        }

        [FilterParameter]
        public string Expression { get; set; }

        [FilterParameter(Name = "w")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? Width { get; set; }

        [FilterParameter(Name = "h")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? Height { get; set; }

        [FilterParameter(Name = "x")]
        [Validate(LogicalOperators.GreaterThanOrEqual, 0)]
        public int? X { get; set; }

        [FilterParameter(Name = "y")]
        [Validate(LogicalOperators.GreaterThanOrEqual, 0)]
        public int? Y { get; set; }

        [FilterParameter(Name = "color")]
        public string Color { get; set; }
    }
}
