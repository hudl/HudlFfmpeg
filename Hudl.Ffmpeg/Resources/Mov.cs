using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Mov : BaseContainer
    {
        private const string FileFormat = ".mov";

        public Mov()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Mov();
        }
    }
}