using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    [ContainsStream(Type = typeof(DataStream))]
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
