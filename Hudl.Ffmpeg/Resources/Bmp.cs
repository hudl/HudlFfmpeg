using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Bmp : BaseImage
    {
        private const string FileFormat = ".bmp";

        public Bmp() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Bmp
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
