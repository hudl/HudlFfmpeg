using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Tiff : BaseImage
    {
        private const string FileFormat = ".tiff";

        public Tiff() 
            : base(FileFormat)
        {
        }
       
        protected override IResource InstanceOfMe()
        {
            return new Tiff
            {
                Id = Id,
                Length = Length,
                Name = Name,
                Path = Path
            };
        }
    }
}
