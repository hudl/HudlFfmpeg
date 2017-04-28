using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// crop fillter will crop the selected filter to a specific size and dimensions
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "crop", MinInputs = 1, MaxInputs = 1)]
    public class Crop : IFilter
    {
        public Crop()
        {
        }
        public Crop(int width, int height)
            : this()
        {
            Width = width; 
            Height = height; 
        }
        public Crop(int width, int height, int x, int y)
            : this(width, height)
        {
            X = x;
            Y = y;
        }

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
    }
}
