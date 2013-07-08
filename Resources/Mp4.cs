using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Mp4 : BaseVideo
    {
        private const string FileFormat = ".mp4";
        public Mp4() 
            : base(FileFormat)
        {
        }
        public Mp4(string path) 
            : base(FileFormat, path)
        {
        }
        public Mp4(string path, TimeSpan length) 
            : base(FileFormat, path, length)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Mp4
            {
                Id = Id,
                Length = Length,
                Path = Path
            };
        }
    }
}
