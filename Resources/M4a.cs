using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class M4A : BaseAudio
    {
        private const string FileFormat = ".m4a";

        public M4A() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new M4A
            {
                Id = Id,
                Length = Length,
                Name = Name,
                Path = Path
            };
        }
    }
}
