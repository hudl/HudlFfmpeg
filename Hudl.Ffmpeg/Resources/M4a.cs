using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class M4A : BaseAudio
    {
        private const string FileFormat = ".m4a";

        public M4A() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new M4A
            {
                Id = Id,
                Info = Info,
                Name = Name,
                Path = Path
            };
        }
    }
}
