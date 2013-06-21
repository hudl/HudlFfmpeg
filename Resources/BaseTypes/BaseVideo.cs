using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseVideo :
        BaseResource,
        IVideo
    {
        public BaseVideo(string format) :
            base(format)
        {
        }
        public BaseVideo(string format, string path) :
            base(format, path)
        {
        }
        public BaseVideo(string format, string path, TimeSpan length) :
            base(format, path, length)
        {
        }

        private new SettingsCollection _settings;
        public readonly SettingsCollection Settings { get { return _settings; } }
    }
}
