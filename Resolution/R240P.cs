using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resolution.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Resolution
{
    public class R240P<TResource> : IResolutionTemplate
        where TResource : IVideo, new()
    {
        public R240P()
        {
            var resolutionFilterchain = Filterchain.FilterTo<TResource>(
                new Scale(ScalePresetTypes.Sd240),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1)));
            var outputSettings = SettingsCollection.ForOutput(
                new Dimensions(ScalePresetTypes.Sd240));

            Filterchains = new List<Filterchain<IResource>> {resolutionFilterchain};
            OutputSettings = outputSettings;
        }

        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }
}
