using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Tests.Resources.Identify
{
    public class Mkv : BaseContainer
    {
        private const string FileFormat = ".mkv";

        public Mkv() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Mkv();
        }

    }
}
