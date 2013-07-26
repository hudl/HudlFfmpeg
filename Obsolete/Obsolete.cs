using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resolution.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

/* 
* Stage 1 Obsolete is a non-error state for objects that have full tested and working solutions. 
* The lifespan of an object in stage 1 obsoletion is 2 months.
*/
#region Stage 1 Obsolete
namespace Hudl.Ffmpeg.Resolution
{
    [Obsolete("Template240P is obsolete, use Resolution240P instead", false)]
    public class Template240P<TResource> : IResolutionTemplate
        where TResource : IVideo, new()
    {
        public Template240P()
        {
            var resolutionFilterchain = Filterchain.FilterTo<TResource>(
                new Scale(ScalePresetType.Sd240),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1)));
            var outputSettings = SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Sd240));

            Filterchains = new List<Filterchain<IResource>> { resolutionFilterchain };
            OutputSettings = outputSettings;
        }

        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }

    [Obsolete("Template360P is obsolete, use Resolution360P instead", false)]
    public class Template360P<TResource> : IResolutionTemplate
        where TResource : IVideo, new()
    {
        public Template360P()
        {
            var resolutionFilterchain = Filterchain.FilterTo<TResource>(
                new Scale(ScalePresetType.Sd360),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1)));
            var outputSettings = SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Sd360),
                new AspectRatio(new FfmpegRatio(16, 9)));

            Filterchains = new List<Filterchain<IResource>> { resolutionFilterchain };
            OutputSettings = outputSettings;
        }

        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }

    [Obsolete("Template480P is obsolete, use Resolution480P instead", false)]
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

            Filterchains = new List<Filterchain<IResource>> { resolutionFilterchain };
            OutputSettings = outputSettings;
        }

        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }

    [Obsolete("Template720P is obsolete, use Resolution720P instead", false)]
    public class Template720P<TResource> : IResolutionTemplate
        where TResource : IVideo, new()
    {
        public Template720P()
        {
            var resolutionFilterchain = Filterchain.FilterTo<TResource>(
                new Scale(ScalePresetType.Hd720),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1)));
            var outputSettings = SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Hd720),
                new AspectRatio(new FfmpegRatio(16, 9)));

            Filterchains = new List<Filterchain<IResource>> { resolutionFilterchain };
            OutputSettings = outputSettings;
        }

        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }

    [Obsolete("Template1080P is obsolete, use Resolution1080P instead", false)]
    public class Template1080P<TResource> : IResolutionTemplate
        where TResource : IVideo, new()
    {
        public Template1080P()
        {
            var resolutionFilterchain = Filterchain.FilterTo<TResource>(
                new Scale(ScalePresetType.Hd1080),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1)));
            var outputSettings = SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Hd1080),
                new AspectRatio(new FfmpegRatio(16, 9)));

            Filterchains = new List<Filterchain<IResource>> { resolutionFilterchain };
            OutputSettings = outputSettings;
        }


        public List<Filterchain<IResource>> Filterchains { get; private set; }

        public SettingsCollection OutputSettings { get; private set; }
    }
}

namespace Hudl.Ffmpeg.Resolution.BaseTypes
{
    [Obsolete("IResolutionTemplate is obsolete, use BaseFilterchainAndSettingsTemplate instead", false)]
    public interface IResolutionTemplate
    {
        List<Filterchain<IResource>> Filterchains { get; }

        SettingsCollection OutputSettings { get; }
    }
}

namespace Hudl.Ffmpeg.Command
{
    partial class Command<TOutput>
        where TOutput : IResource
    {
        [Obsolete("SetResolution is obsolete, use ApplyFilter and Settings instead", false)]
        public void SetResolution(IResolutionTemplate resolutionTemplate)
        {
            //assign the resolution template to all input resources as the first line of filterchain
            var allInputReceipts = GetResourceReceipts();
            allInputReceipts.ForEach(receipt => resolutionTemplate.Filterchains.ForEach(filterchain =>
            {
                var newFilterchain = filterchain.Copy<IResource>();
                newFilterchain.SetResources(receipt);
                var newReceipt = new CommandResourceReceipt(Parent.Id, Id, newFilterchain.Output.Resource.Map);
                Filtergraph.FilterchainList.ForEach(otherFilterchain =>
                {
                    if (otherFilterchain.ResourceList.Any(r => r.Map == receipt.Map))
                    {
                        var currentIndex = otherFilterchain.ResourceList.FindIndex(r => r.Map == receipt.Map);
                        otherFilterchain.ResourceList[currentIndex] = newReceipt;
                    }
                });
                Filtergraph.FilterchainList.Insert(0, newFilterchain);
            }));

            //assign and merge the output resolutio settings for the resolution saying that the new wins 
            Output.Settings.MergeRange(resolutionTemplate.OutputSettings, FfmpegMergeOptionType.NewWins);
        }

    }
}
#endregion


/* 
* Stage 2 Obsolete is an error state for objects that have full tested and working solutions. 
* The lifespan of an object in stage 1 obsoletion is 2 months.
*/
#region Stage 2 Obsolete
#endregion 

/* 
* Stage 3 Obsolete is to remove the code entirely
*/