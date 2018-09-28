using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(SubtitleStream))]
    public class Ass : BaseContainer
    {
        private const string FileFormat = ".ass";

        public Ass()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Ass();
        }
    }
}