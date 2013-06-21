using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class M4a : 
        BaseAudio
    {
        private const string FileFormat = ".m4a";
        public M4a() :
            base(FileFormat)
        {
        }
        public M4a(string path) :
            base(FileFormat, path)
        {
        }
        public M4a(string path, TimeSpan length) :
            base(FileFormat, path, length)
        {
        }
    }
}
