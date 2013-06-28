using System;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseAudio :
        BaseResource,
        IAudio
    {
        protected BaseAudio(string format) 
            : base(format)
        {
            Settings = new SettingsCollection();
        }
        protected BaseAudio(string format, string path) 
            : base(format, path)
        {
            Settings = new SettingsCollection();
        }
        protected BaseAudio(string format, string path, TimeSpan length) 
            : base(format, path, length)
        {
            Settings = new SettingsCollection();
        }

        public SettingsCollection Settings { get; set; }
    }
}
