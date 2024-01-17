using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    [ContainsStream(Type = typeof(DataStream))]
    public class M4s : BaseContainer
    {
        private const string FileFormat = ".m4s";

        public M4s()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new M4s();
        }
    }
}