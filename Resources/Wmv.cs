using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Wmv : BaseVideo
    {
        private const string FileFormat = ".wmv";

        public Wmv() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Wmv
            {
                Id = Id,
                Length = Length,
                Name = Name,
                Path = Path
            };
        }
    }
}
