using System;
using Hudl.Ffmpeg.Settings.BaseTypes;

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
            Settings = SettingsCollection.ForInput();
        }
        protected BaseAudio(string format, string path) 
            : base(format, ResourceIndicatorType, path)
        {
            Settings = SettingsCollection.ForInput();
        }
        protected BaseAudio(string format, string path, TimeSpan length) 
            : base(format, ResourceIndicatorType, path, length)
        {
            Settings = SettingsCollection.ForInput();
        }

        public SettingsCollection Settings { get; set; }
    }
}
