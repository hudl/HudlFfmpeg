using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    interface IVideo :
        IResource,
        ISettingable
    {
    }
}
