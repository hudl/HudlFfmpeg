using System;
using Hudl.Ffmpeg.Settings.BaseTypes;

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
            Settings = SettingsCollection.ForInput();
        }
        protected BaseVideo(string format, string path)
            : base(format, ResourceIndicatorType, path)
        {
            Settings = SettingsCollection.ForInput();
        }
        protected BaseVideo(string format, string path, TimeSpan length)
            : base(format, ResourceIndicatorType, path, length)
        {
            Settings = SettingsCollection.ForInput();
        }

        public SettingsCollection Settings { get; set; }
    }
}
