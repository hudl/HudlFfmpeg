using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(SubtitleStream))]
    public class Srt : BaseContainer
    {
        private const string FileFormat = ".srt";

        public Srt()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Srt();
        }
    }
}
