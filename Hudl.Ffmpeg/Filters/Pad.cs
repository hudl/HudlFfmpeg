using System;
using System.Drawing;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Filter that applies padding to input video 
    /// </summary>
    [AppliesToResource(Type = typeof(IImage))]
    [AppliesToResource(Type = typeof(IVideo))]
    public class Pad : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "pad";

        public const string ExprConvertTo169Aspect = "ih*16/9:ih:(ow-iw)/2:(oh-ih)/2"; 

        public Pad() 
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Pad(Size toDimensions, Point atPosition)
            : this()
        {
            To = toDimensions;
            Position = atPosition;
        }
        public Pad(string expression)
            : this()
        {
            Expression = expression;
        }

        public string Expression { get; set; }

        public Size To { get; set; }

        public Point Position { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                if (To.IsEmpty)
                {
                    throw new InvalidOperationException("To dimensions cannot be null.");
                }
                if (Position.IsEmpty)
                {
                    throw new InvalidOperationException("Position point cannot be null.");
                }
            }
        }

        public override string ToString() 
        {
            if (!string.IsNullOrWhiteSpace(Expression))
            {
                return string.Concat(Type, "=", Expression);
            }

            return string.Format("{0}=width={1}:height={2}:x={3}:y={4}", 
                Type,
                To.Width, 
                To.Height,
                Position.X, 
                Position.Y);
        }
    }
}
