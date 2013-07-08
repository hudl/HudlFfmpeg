using System;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseImage :
        BaseResource,
        IImage
    {
        private const string ResourceIndicatorType = "v";

        protected BaseImage(string format) 
            : base(format, ResourceIndicatorType)
        {
        }
        protected BaseImage(string format, string path) 
            : base(format, ResourceIndicatorType, path)
        {
        }
        protected BaseImage(string format, string path, TimeSpan length) 
            : base(format, ResourceIndicatorType, path, length)
        {
        }
    }
}
