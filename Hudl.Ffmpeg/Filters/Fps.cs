using System;
using Hudl.Ffmpeg.BaseTypes;
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

        public override void Validate()
        {
            if (FrameRate <= 0)
            {
                throw new InvalidOperationException("FrameRate must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, "=", FrameRate);
        }
    }
}
