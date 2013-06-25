using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Png : 
        BaseImage
    {
        private const string FileFormat = ".png";
        public Png() :
            base(FileFormat)
        {
        }
        public Png(string path) :
            base(FileFormat, path)
        {
        }
    }
}
