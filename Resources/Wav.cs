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
        public Wav(string path) 
            : base(FileFormat, path)
        {
        }
        public Wav(string path, TimeSpan length) 
            : base(FileFormat, path, length)
        {
        }

        public override IResource Copy()
        {
            return Copy<Wav>();
        }
    }
}
