using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(VideoStream))]
    [ContainsStream(Type = typeof(DataStream))]
    public class Png : BaseContainer
    {
        private const string FileFormat = ".png";

        public Png() 
            : base(FileFormat)
        {
        }
       
        protected override IContainer Clone()
        {
            return new Png();
        }
    }
}
