using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
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
