using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    public class M4A : BaseContainer
    {
        private const string FileFormat = ".m4a";

        public M4A() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new M4A();
        }
    }
}
