using System;
using System.Drawing;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// crop fillter will crop the selected filter to a specific size and dimensions
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
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

        public override string ToString()
        {
            if (Dimensions.Width <= 0)
            {
                throw new InvalidOperationException("Dimensions.Width must be greater than zero for cropping.");
            }
            if (Dimensions.Height <= 0)
            {
                throw new InvalidOperationException("Dimensions.Height must be greater than zero for cropping.");
            }

            var filter = new StringBuilder(100);
            if (Dimensions.Width != 0)
            {
                filter.AppendFormat("{1}w={0}",
                    Dimensions.Width,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Dimensions.Height != 0)
            {
                filter.AppendFormat("{1}h={0}",
                    Dimensions.Height,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Offset.X != 0)
            {
                filter.AppendFormat("{1}x={0}",
                    Offset.X,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Offset.Y != 0)
            {
                filter.AppendFormat("{1}y={0}",
                    Offset.Y,
                    (filter.Length > 0) ? ":" : string.Empty);
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
