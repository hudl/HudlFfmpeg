using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Sgi : BaseImage
    {
        private const string FileFormat = ".sgi";

        public Sgi() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Sgi
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
