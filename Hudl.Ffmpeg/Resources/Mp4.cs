using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Mp4 : BaseContainer
    {
        private const string FileFormat = ".mp4";

        public Mp4() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Mp4();
        }
    }
}
