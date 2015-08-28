using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Ppm : BaseImage
    {
        private const string FileFormat = ".ppm";

        public Ppm() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Ppm
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
