using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Ismv : BaseVideo
    {
        private const string FileFormat = ".ismv";

        public Ismv()
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Ismv
            {
                Id = Id,
                Info = Info,
                Name = Name,
                Path = Path
            };
        }
    }
}