using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// interface that forces a type to expose it's Settings interface 
    /// </summary>
    interface ISettingable
    {
        SettingsCollection Settings { get; set; }
    }
}
