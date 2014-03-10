using System;
using System.Text;
using System.Drawing;
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
            CropTo = new Size(0, 0);
            PositionAt = new Point(0, 0);
        }
        public Crop(int width, int height)
            : this()
        {
            CropTo = new Size(width, height);
        }
        public Crop(Size cropTo, Point positionAt)
            : this()
        {
            CropTo = cropTo; 
            PositionAt = positionAt; 
        }
        public Crop(int width, int height, int x, int y)
            : this()
        {
            PositionAt = new Point(x, y);
            CropTo = new Size(width, height);
        }


        public Point PositionAt { get; set; }

        public Size CropTo { get; set; }

        public override string ToString()
        {
            if (CropTo.Width <= 0)
            {
                throw new InvalidOperationException("CropTo.Width must be greater than zero for cropping.");
            }
            if (CropTo.Height <= 0)
            {
                throw new InvalidOperationException("CropTo.Height must be greater than zero for cropping.");
            }

            var filter = new StringBuilder(100);
            if (CropTo.Width != 0)
            {
                filter.AppendFormat("{1}w={0}",
                    CropTo.Width,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (CropTo.Height != 0)
            {
                filter.AppendFormat("{1}h={0}",
                    CropTo.Height,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (PositionAt.X != 0)
            {
                filter.AppendFormat("{1}x={0}",
                    PositionAt.X,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (PositionAt.Y != 0)
            {
                filter.AppendFormat("{1}y={0}",
                    PositionAt.Y,
                    (filter.Length > 0) ? ":" : string.Empty);
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
