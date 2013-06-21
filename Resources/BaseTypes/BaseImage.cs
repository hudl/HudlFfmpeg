using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseImage :
        BaseResource,
        IImage
    {
        public BaseImage(string format) :
            base(format)
        {
        }
        public BaseImage(string format, string path) :
            base(format, path)
        {
        }
        public BaseImage(string format, string path, TimeSpan length) :
            base(format, path, length)
        {
        }
    }
}
