using System;
using System.Text;
using System.Drawing;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// crop fillter will crop the selected filter to a specific size and dimensions
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    public class Crop : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "crop";

        public Crop()
            : base(FilterType, FilterMaxInputs)
        {
            Dimensions = new Size(0, 0);
            Offset = new Point(0, 0);
        }
        public Crop(int width, int height)
            : this()
        {
            Dimensions = new Size(width, height);
        }
        public Crop(int width, int height, int x, int y)
            : this()
        {
            Offset = new Point(x, y);
            Dimensions = new Size(width, height);
        }
        public Crop(Size dimensions, Point offset) 
            : this()
        {
            Offset = offset;
            Dimensions = dimensions; 
        }

        public Point Offset { get; set; }

        public Size Dimensions { get; set; }

        public override void Validate()
        {
            if (Dimensions.Width <= 0)
            {
                throw new InvalidOperationException("Dimensions.Width must be greater than zero for cropping.");
            }
            if (Dimensions.Height <= 0)
            {
                throw new InvalidOperationException("Dimensions.Height must be greater than zero for cropping.");
            }
            if (Offset.X < 0)
            {
                throw new InvalidOperationException("Offset.X must be greater than or equal to zero for cropping.");
            }
            if (Offset.Y < 0)
            {
                throw new InvalidOperationException("Offset.Y must be greater than or equal to zero for cropping.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (Dimensions.Width != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "w", Dimensions.Width);
            }
            if (Dimensions.Height != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "h", Dimensions.Height);
            }
            if (Offset.X != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "x", Offset.X);
            }
            if (Offset.Y != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "y", Offset.Y);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
