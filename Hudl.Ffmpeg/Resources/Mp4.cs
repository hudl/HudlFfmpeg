using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Mp4 : BaseVideo
    {
        private const string FileFormat = ".mp4";

        public Mp4() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Mp4
            {
                Id = Id,
                Info = Info,
                Name = Name,
                Path = Path
            };
        }
    }
}
