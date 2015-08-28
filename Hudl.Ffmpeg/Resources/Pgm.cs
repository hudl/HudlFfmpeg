using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Pgm : BaseImage
    {
        private const string FileFormat = ".pgm";

        public Pgm() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Pgm
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
