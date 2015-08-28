using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Pcx : BaseImage
    {
        private const string FileFormat = ".pcx";

        public Pcx() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Pcx
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
