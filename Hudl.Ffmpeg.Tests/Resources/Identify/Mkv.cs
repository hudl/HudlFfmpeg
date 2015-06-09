using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Tests.Resources.Identify
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
