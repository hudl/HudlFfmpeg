using System;
using System.Drawing;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
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
            Dimensions = new Point(0, 0);
            Offset = new Point(0, 0);
        }
        public Crop(int width, int height)
            : this()
        {
            Dimensions = new Point(width, height);
        }
        public Crop(int width, int height, int x, int y)
            : this()
        {
            Offset = new Point(x, y);
            Dimensions = new Point(width, height);
        }

        public Point Offset { get; set; }

        public Point Dimensions { get; set; }

        public override string ToString()
        {
            if (Dimensions.X <= 0)
            {
                throw new InvalidOperationException("Dimensions.X must be greater than zero for cropping.");
            }
            if (Dimensions.Y <= 0)
            {
                throw new InvalidOperationException("Dimensions.Y must be greater than zero for cropping.");
            }

            var filter = new StringBuilder(100);
            if (Dimensions.X != 0)
            {
                filter.AppendFormat("{1}w={0}",
                    Dimensions.X,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Dimensions.Y != 0)
            {
                filter.AppendFormat("{1}h={0}",
                    Dimensions.Y,
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
