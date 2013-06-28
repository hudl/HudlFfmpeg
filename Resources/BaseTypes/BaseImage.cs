using System;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseImage :
        BaseResource,
        IImage
    {
        protected BaseImage(string format) 
            : base(format)
        {
        }
        protected BaseImage(string format, string path) 
            : base(format, path)
        {
        }
        protected BaseImage(string format, string path, TimeSpan length) 
            : base(format, path, length)
        {
        }
    }
}
