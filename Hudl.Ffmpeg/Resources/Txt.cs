using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Txt : BaseContainer
    {
        private const string FileFormat = ".txt";

        public Txt()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Txt();
        }
    }
}