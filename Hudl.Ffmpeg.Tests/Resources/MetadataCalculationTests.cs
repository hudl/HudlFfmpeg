using System;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Filters.Templates;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Sugar;
using Xunit;
using Utilities = Hudl.Ffmpeg.Tests.Assets.Utilities;

namespace Hudl.Ffmpeg.Tests.Resources
{
    public class MetadataCalculationTests
    {
        public MetadataCalculationTests()
        {
            Utilities.SetGlobalAssets();
        }

        [Fact]
        public void InputToOutput_Verify()
        {
            var baseline = Resource.From(Utilities.GetVideoFile()).LoadMetadata().Info;

            var output = CommandFactory.Create()
                                       .CreateOutputCommand()
                                       .WithInput(Utilities.GetVideoFile())
                                       .To<Mp4>()
                                       .First();

            var metadataInfo = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetReceipt());

            Assert.True(metadataInfo.Duration == baseline.Duration);

        }

        [Fact]
        public void InputSettingsToOutput_Verify()
        {
            var inputSettings = SettingsCollection.ForInput(
                new StartAt(1d),
                new DurationInput(3d)); 

            var output = CommandFactory.Create()
                                       .CreateOutputCommand()
                                       .WithInput(Utilities.GetVideoFile(), inputSettings)
                                       .To<Mp4>()
                                       .First();

            var metadataInfo = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetReceipt());

            Assert.True(metadataInfo.Duration == TimeSpan.FromSeconds(3));
        }

        [Fact]
        public void InputSettingsToOutputSettings_Verify()
        {
            var inputSettings = SettingsCollection.ForInput(new StartAt(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000), 
                new DurationOutput(5d)); 

            var output = CommandFactory.Create()
                                       .CreateOutputCommand()
                                       .WithInput(Utilities.GetVideoFile(), inputSettings)
                                       .To<Mp4>(outputSettings)
                                       .First();

            var metadataInfo = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetReceipt());

            Assert.True(metadataInfo.Duration == TimeSpan.FromSeconds(3)); // this is because the input is only 4 seconds, we start at 1
            Assert.True(metadataInfo.BitRate == 3000000L);
        }

        [Fact]
        public void InputToFilterToOutput_Verify()
        {
            var baseline = Resource.From(Utilities.GetVideoFile()).LoadMetadata().Info;
            var calculatedSeconds = (baseline.Duration.TotalSeconds * 3d) - 2d;
            var calculatedDuration = TimeSpan.FromSeconds(calculatedSeconds);

            var filterchain = Filterchain.FilterTo<Mp4>(new Split(3));

            var split = CommandFactory.Create()
                                      .CreateOutputCommand()
                                      .WithInput(Utilities.GetVideoFile())
                                      .Filter(filterchain);

            var concat1 = split.Command
                               .WithStreams(split.Receipts[1])
                               .WithStreams(split.Receipts[1])
                               .Filter(new CrossfadeConcatenate<Mp4>(1d));

            var concat2 = concat1.WithStreams(split.Receipts[2])
                                 .Filter(new CrossfadeConcatenate<Mp4>(1d));

            var output = concat2.MapTo<Mp4>().First(); 

            var metadataInfo1 = MetadataHelpers.GetMetadataInfo(concat2.Command, concat2.Receipts.FirstOrDefault());
            var metadataInfo2 = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetReceipt());

            Assert.True(metadataInfo1.Duration == calculatedDuration);
            Assert.True(metadataInfo2.Duration == calculatedDuration);
        }

        [Fact]
        public void InputSettingsToFilterToOutputSettings_Verify()
        {
            var baseline = Resource.From(Utilities.GetVideoFile()).LoadMetadata().Info;
            var calculatedSeconds = ((baseline.Duration.TotalSeconds - 1) * 3d) - 2d;
            var calculatedDuration = TimeSpan.FromSeconds(calculatedSeconds);

            var inputSettings = SettingsCollection.ForInput(new StartAt(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000),
                new DurationOutput(5d)); 

            var filterchain = Filterchain.FilterTo<Mp4>(new Split(3));

            var split = CommandFactory.Create()
                                      .CreateOutputCommand()
                                      .WithInput(Utilities.GetVideoFile(), inputSettings)
                                      .Filter(filterchain);

            var concat1 = split.Command
                               .WithStreams(split.Receipts[0])
                               .WithStreams(split.Receipts[1])
                               .Filter(new CrossfadeConcatenate<Mp4>(1d));

            var concat2 = concat1.WithStreams(split.Receipts[2])
                                 .Filter(new CrossfadeConcatenate<Mp4>(1d));

            var output = concat2.MapTo<Mp4>(outputSettings).First();

            var metadataInfo1 = MetadataHelpers.GetMetadataInfo(concat2.Command, concat2.Receipts.FirstOrDefault());
            var metadataInfo2 = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetReceipt());

            Assert.True(metadataInfo1.Duration == calculatedDuration);
            Assert.True(metadataInfo2.Duration == TimeSpan.FromSeconds(5));
            Assert.True(metadataInfo2.BitRate == 3000000L);
        }
    }
}
