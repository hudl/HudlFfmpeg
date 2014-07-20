using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    public class Mp3 : BaseContainer
    {
        private const string FileFormat = ".mp3";

        public Mp3() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Mp3(); 
        }
    }
}
