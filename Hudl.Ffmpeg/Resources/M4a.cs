using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
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
