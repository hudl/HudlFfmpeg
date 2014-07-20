using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Ismv : BaseContainer
    {
        private const string FileFormat = ".ismv";

        public Ismv()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Ismv();
        }
    }
}