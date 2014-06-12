using System;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseVideo :
        BaseResource,
        IVideo
    {
        private const string ResourceIndicatorType = "v";

        protected BaseVideo(string format)
            : base(format, ResourceIndicatorType)
        {
        }
        protected BaseVideo(string format, string name)
            : base(format, ResourceIndicatorType, name)
        {
        }
        protected BaseVideo(string format, string name, string path)
            : base(format, ResourceIndicatorType, name, path)
        {
        }
        protected BaseVideo(string format, string name, TimeSpan length)
            : base(format, ResourceIndicatorType, name, length)
        {
        }
        protected BaseVideo(string format, string name, string path, TimeSpan length)
            : base(format, ResourceIndicatorType, name, path, length)
        {
        }
    }
}
