using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Wav : 
        BaseAudio
    {
        private const string FileFormat = ".wav";
        public Wav() :
            base(FileFormat)
        {
        }
        public Wav(string path) :
            base(FileFormat, path)
        {
        }
        public Wav(string path, TimeSpan length) :
            base(FileFormat, path, length)
        {
        }
    }
}
