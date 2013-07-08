using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Png : BaseImage
    {
        private const string FileFormat = ".png";
        public Png() 
            : base(FileFormat)
        {
        }
        public Png(string path) 
            : base(FileFormat, path)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Png
            {
                Id = Id,
                Length = Length,
                Path = Path
            };
        }
    }
}
