using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Mp4 : 
        BaseVideo
    {
        private const string FileFormat = ".mp4";
        public Mp4() :
            base(FileFormat)
        {
        }
        public Mp4(string path) :
            base(FileFormat, path)
        {
        }
        public Mp4(string path, TimeSpan length) :
            base(FileFormat, path, length)
        {
        }
    }
}
