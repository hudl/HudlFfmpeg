using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Wav : BaseAudio
    {
        private const string FileFormat = ".wav";
        
        public Wav() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Wav
            {
                Id = Id,
                Info = Info,
                Name = Name,
                Path = Path
            };
        }
    }
}
