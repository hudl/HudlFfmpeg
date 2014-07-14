using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Ts : BaseVideo
    {
        private const string FileFormat = ".ts";

        public Ts()
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Ts
            {
                Id = Id,
                Info = Info,
                Name = Name,
                Path = Path
            };
        }
    }
}