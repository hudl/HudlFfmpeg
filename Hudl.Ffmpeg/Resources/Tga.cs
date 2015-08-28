using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Tga : BaseImage
    {
        private const string FileFormat = ".tga";

        public Tga() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Tga
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
