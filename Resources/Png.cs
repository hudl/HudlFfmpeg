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

        public override IResource Copy()
        {
            return Copy<Png>();
        }
    }
}
