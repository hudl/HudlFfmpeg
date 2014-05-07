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
    /// fps filter will set a frames per second on the stream, can be used on images as well
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    public class Fps : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "fps";

        public Fps()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Fps(int frameRate)
            : this()
        {
            FrameRate = frameRate;
        }

        public int FrameRate { get; set; }

        public override string ToString()
        {
            if (FrameRate <= 0) 
            {
                throw new InvalidOperationException("FrameRate must be greater than zero.");
            }

            return string.Concat(Type, "=", FrameRate);
        }
    }
}
