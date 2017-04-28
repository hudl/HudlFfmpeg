using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(DataStream))]
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
