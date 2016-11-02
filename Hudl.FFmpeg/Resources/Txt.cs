using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    [ContainsStream(Type = typeof(DataStream))]
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