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
    public class Template480P<TResource> : IResolutionTemplate
        where TResource : IVideo, new()
    {
        public Template480P()
        {
            var resolutionFilterchain = Filters.BaseTypes.Filterchain.FilterTo<TResource>(
                new Scale(ScalePresetType.Hd480),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1)));
            var outputSettings = SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Hd480), 
                new AspectRatio(new FfmpegRatio(16, 9)));

            Filterchains = new List<Filterchain<IResource>> {resolutionFilterchain};
            OutputSettings = outputSettings;
        }

        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }
}
