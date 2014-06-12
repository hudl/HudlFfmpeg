using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    public class Resolution1080P : BaseFilterchainAndSettingsTemplate
    {
        private Resolution1080P(IResource resourceToUse)
            : base(resourceToUse, SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Hd1080),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Size(ScalePresetType.Hd1080),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }

        public static Resolution1080P Create<TResource>()
            where TResource : IVideo, new()
        {
            return Create(new TResource());
        }
        public static Resolution1080P Create(IResource resourceToUse)
        {
            if (resourceToUse == null)
            {
                throw new ArgumentNullException("resourceToUse");
            }

            return new Resolution1080P(resourceToUse);
        }
    }
}
