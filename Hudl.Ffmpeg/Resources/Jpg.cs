using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(VideoStream))]
    public class Jpg : BaseContainer
    {
        private const string FileFormat = ".jpg";

        public Jpg() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Jpg();
        }
    }
}
