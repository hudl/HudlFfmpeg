using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Pbm : BaseImage
    {
        private const string FileFormat = ".pbm";

        public Pbm() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Pbm
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
