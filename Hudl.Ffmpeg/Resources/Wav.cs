using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Resources
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
