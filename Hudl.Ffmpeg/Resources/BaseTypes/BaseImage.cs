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
        protected BaseImage(string format, string name)
            : base(format, ResourceIndicatorType, name)
        {
        }
        protected BaseImage(string format, string name, string path)
            : base(format, ResourceIndicatorType, name, path)
        {
        }
        protected BaseImage(string format, string name, TimeSpan length)
            : base(format, ResourceIndicatorType, name, length)
        {
        }
        protected BaseImage(string format, string name, string path, TimeSpan length)
            : base(format, ResourceIndicatorType, name, path, length)
        {
        }
    }
}
