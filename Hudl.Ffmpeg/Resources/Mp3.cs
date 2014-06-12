using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Mp3 : BaseAudio
    {
        private const string FileFormat = ".mp3";

        public Mp3() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Mp3
            {
                Id = Id,
                Info = Info,
                Name = Name,
                Path = Path
            };
        }
    }
}
