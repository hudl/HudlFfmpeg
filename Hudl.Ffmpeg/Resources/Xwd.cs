using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Xwd : BaseImage
    {
        private const string FileFormat = ".xwd";

        public Xwd() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Xwd
                {
                    Id = Id, 
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}
