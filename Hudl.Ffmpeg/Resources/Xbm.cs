using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Xbm : BaseImage
    {
        private const string FileFormat = ".xbm";

        public Xbm() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Xbm
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
