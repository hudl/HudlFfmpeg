using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Mkv : BaseContainer
    {
        private const string FileFormat = ".mkv";

        public Mkv()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Mkv();
        }
    }
}