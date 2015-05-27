using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Ts : BaseContainer
    {
        private const string FileFormat = ".ts";

        public Ts()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Ts();
        }
    }
}