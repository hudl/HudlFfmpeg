using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Video filter that rotates video by an arbitrary angle.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "rotate", MinInputs = 1, MaxInputs = 1)]
    public class Rotate : IFilter
    {
        public Rotate()
        {
        }
        public Rotate(double angleDegrees)
            : this(angleDegrees, null, null)
        {
        }

        public Rotate(double angleDegrees, string outputWidth, string outputHeight)
            : this(String.Format("{0}*PI/180", angleDegrees), outputWidth, outputHeight)
        {
        }

        public Rotate(string angleExpression)
            : this(angleExpression, null, null)
        {
        }

        public Rotate(string angleExpression, string outputWidth, string outputHeight)
        {
            Angle = angleExpression;
            OutputWidth = outputWidth;
            OutputHeight = outputHeight;
        }

        [FilterParameter(Name = "a", Default = "0")]
        public string Angle { get; set; }

        [FilterParameter(Name = "ow", Default = "iw")]
        public string OutputWidth { get; set; }

        [FilterParameter(Name = "oh", Default = "ih")]
        public string OutputHeight { get; set; }

        [FilterParameter(Name = "c", Default = "black")]
        public string FillColor { get; set; }

        [FilterParameter(Name = "bilinear", Default = true, Formatter = typeof(BoolToInt32Formatter))]
        public bool Bilinear { get; set; }
    }
}
