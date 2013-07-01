using System;
using System.Drawing;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Output)]
    public class Dimensions : BaseSetting
    {
        private const string SettingType = "-s";

        public Dimensions(Point size)
            : base(SettingType)
        {
            if (size == null)
            {
                throw new ArgumentNullException("size");
            }

            Size = size;
        }
        public Dimensions(int width, int height)
            : this(new Point(width, height))
        {
            if (width <= 0)
            {
                throw new ArgumentException("Dimensions width must be greater than zero.");
            }
            if (height <= 0)
            {
                throw new ArgumentException("Dimensions height must be greater than zero.");
            }
        }

        public Point Size { get; set; }

        public override string ToString()
        {
            if (Size == null)
            {
                throw new ArgumentException("Dimensions size cannot be null.");
            }
            if (Size.X <= 0)
            {
                throw new ArgumentException("Dimensions width must be greater than zero.");
            }
            if (Size.Y <= 0)
            {
                throw new ArgumentException("Dimensions height must be greater than zero.");
            }

            return string.Concat(Type, " ", Size.X, "x", Size.Y);
        }
    }
}
