using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(VideoStream))]
    [ContainsStream(Type = typeof(DataStream))]
    public class Bmp : BaseContainer
    {
        private const string FileFormat = ".Bmp";

        public Bmp() 
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Bmp();
        }
    }
}
