using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(VideoStream))]
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
