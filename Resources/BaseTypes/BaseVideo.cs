using System;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseVideo :
        BaseResource,
        IVideo
    {
        protected BaseVideo(string format) 
            : base(format)
        {
            Settings = SettingsCollection.ForInput();
        }
        protected BaseVideo(string format, string path) 
            : base(format, path)
        {
            Settings = SettingsCollection.ForInput();
        }
        protected BaseVideo(string format, string path, TimeSpan length) 
            : base(format, path, length)
        {
            Settings = SettingsCollection.ForInput();
        }

        public SettingsCollection Settings { get; set; }
    }
}
