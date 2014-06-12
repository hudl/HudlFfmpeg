using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    public class Resolution480P : BaseFilterchainAndSettingsTemplate
    {
        private Resolution480P(IResource resourceToUse)
            : base(resourceToUse, SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Sd480),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Size(ScalePresetType.Sd480),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }

        public static Resolution480P Create<TResource>()
            where TResource : IVideo, new()
        {
            return Create(new TResource());
        }
        public static Resolution480P Create(IResource resourceToUse)
        {
            if (resourceToUse == null)
            {
                throw new ArgumentNullException("resourceToUse");
            }

            return new Resolution480P(resourceToUse);
        }
    }
}