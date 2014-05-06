using System;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseAudio :
        BaseResource,
        IAudio
    {
        private const string ResourceIndicatorType = "a";

        protected BaseAudio(string format)
            : base(format, ResourceIndicatorType)
        {
        }
        protected BaseAudio(string format, string name)
            : base(format, ResourceIndicatorType, name)
        {
        }
        protected BaseAudio(string format, string name, string path)
            : base(format, ResourceIndicatorType, name, path)
        {
        }
        protected BaseAudio(string format, string name, TimeSpan length)
            : base(format, ResourceIndicatorType, name, length)
        {
        }
        protected BaseAudio(string format, string name, string path, TimeSpan length)
            : base(format, ResourceIndicatorType, name, path, length)
        {
        }
    }
}
