using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Gif : BaseImage
    {
        private const string FileFormat = ".gif";

        public Gif() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Gif
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
