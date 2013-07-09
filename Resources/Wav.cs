using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Wav : BaseAudio
    {
        private const string FileFormat = ".wav";
        
        public Wav() 
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Wav
            {
                Id = Id,
                Length = Length,
                Name = Name,
                Path = Path
            };
        }
    }
}
