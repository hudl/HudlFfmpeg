using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resolution.BaseTypes
{
    public interface IResolutionTemplate
    {
        List<Filterchain<IResource>> Filterchains { get; }

        SettingsCollection OutputSettings { get; }
    }
}
