using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Wmv : BaseContainer
    {
        private const string FileFormat = ".wmv";

        public Wmv() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Wmv();
        }
    }
}
