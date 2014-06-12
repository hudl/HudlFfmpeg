using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Jpg : BaseImage
    {
        private const string FileFormat = ".jpg";

        public Jpg() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Jpg
                {
                    Id = Id, 
                    Info = Info,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
