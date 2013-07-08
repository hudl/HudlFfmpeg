using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Mp3 : BaseAudio
    {
        private const string FileFormat = ".mp3";
        public Mp3() 
            : base(FileFormat)
        {
        }
        public Mp3(string path) 
            : base(FileFormat, path)
        {
        }
        public Mp3(string path, TimeSpan length) 
            : base(FileFormat, path, length)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Mp3
            {
                Id = Id,
                Length = Length,
                Path = Path
            };
        }
    }
}
