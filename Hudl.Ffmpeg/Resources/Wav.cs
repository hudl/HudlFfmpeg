using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    public class Wav : BaseContainer
    {
        private const string FileFormat = ".wav";
        
        public Wav() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Wav();
        }
    }
}
