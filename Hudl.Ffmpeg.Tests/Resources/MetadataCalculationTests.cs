using System;
using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Templates;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Xunit;
using Utilities = Hudl.FFmpeg.Tests.Assets.Utilities;

namespace Hudl.FFmpeg.Tests.Resources
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
            var baseline = Resource.From(Utilities.GetVideoFile())
                                   .LoadMetadata()
                                   .Streams
                                   .OfType<VideoStream>()
                                   .First();

            var output = CommandFactory.Create()
                                       .CreateOutputCommand()
                                       .WithInput<VideoStream>(Utilities.GetVideoFile())
                                       .To<Mp4>()
                                       .First();

            var metadataInfo = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetStreamIdentifier());

            Assert.True(metadataInfo.VideoStream.Duration == baseline.Info.Duration);

        }

        [Fact]
        public void InputSettingsToOutput_Verify()
        {
            var inputSettings = SettingsCollection.ForInput(
                new StartAt(1d),
                new DurationInput(3d)); 

            var output = CommandFactory.Create()
                                       .CreateOutputCommand()
                                       .WithInput<VideoStream>(Utilities.GetVideoFile(), inputSettings)
                                       .To<Mp4>()
                                       .First();

            var metadataInfo = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetStreamIdentifier());

            Assert.True(metadataInfo.VideoStream.Duration == TimeSpan.FromSeconds(3));
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
                                       .WithInput<VideoStream>(Utilities.GetVideoFile(), inputSettings)
                                       .To<Mp4>(outputSettings)
                                       .First();

            var metadataInfo = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetStreamIdentifier());

            Assert.True(metadataInfo.VideoStream.Duration == TimeSpan.FromSeconds(3)); // this is because the input is only 4 seconds, we start at 1
            Assert.True(metadataInfo.VideoStream.BitRate == 3000000L);
        }

        [Fact]
        public void InputToFilterToOutput_Verify()
        {
            var baseline = Resource.From(Utilities.GetVideoFile())
                                   .LoadMetadata()
                                   .Streams
                                   .OfType<VideoStream>()
                                   .First();
            var calculatedSeconds = (baseline.Info.Duration.TotalSeconds * 3d) - 2d;
            var calculatedDuration = TimeSpan.FromSeconds(calculatedSeconds);

            var filterchain = Filterchain.FilterTo<VideoStream>(new Split(3));

            var split = CommandFactory.Create()
                                      .CreateOutputCommand()
                                      .WithInput<VideoStream>(Utilities.GetVideoFile())
                                      .Filter(filterchain);

            var concat1 = split.Command
                               .Select(split.StreamIdentifiers[1])
                               .Select(split.StreamIdentifiers[1])
                               .Filter(new CrossfadeConcatenate(1d));

            var concat2 = concat1.Select(split.StreamIdentifiers[2])
                                 .Filter(new CrossfadeConcatenate(1d));

            var output = concat2.MapTo<Mp4>().First(); 

            var metadataInfo1 = MetadataHelpers.GetMetadataInfo(concat2.Command, concat2.StreamIdentifiers.FirstOrDefault());
            var metadataInfo2 = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetStreamIdentifier());

            Assert.True(metadataInfo1.VideoStream.Duration == calculatedDuration);
            Assert.True(metadataInfo2.VideoStream.Duration == calculatedDuration);
        }

        [Fact]
        public void InputSettingsToFilterToOutputSettings_Verify()
        {
            var baseline = Resource.From(Utilities.GetVideoFile())
                                   .LoadMetadata()
                                   .Streams
                                   .OfType<VideoStream>()
                                   .First();
            var calculatedSeconds = ((baseline.Info.Duration.TotalSeconds - 1) * 3d) - 2d;
            var calculatedDuration = TimeSpan.FromSeconds(calculatedSeconds);

            var inputSettings = SettingsCollection.ForInput(new StartAt(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000),
                new DurationOutput(5d)); 

            var filterchain = Filterchain.FilterTo<VideoStream>(new Split(3));

            var split = CommandFactory.Create()
                                      .CreateOutputCommand()
                                      .WithInput<VideoStream>(Utilities.GetVideoFile(), inputSettings)
                                      .Filter(filterchain);

            var concat1 = split.Command
                               .Select(split.StreamIdentifiers[0])
                               .Select(split.StreamIdentifiers[1])
                               .Filter(new CrossfadeConcatenate(1d));

            var concat2 = concat1.Select(split.StreamIdentifiers[2])
                                 .Filter(new CrossfadeConcatenate(1d));

            var output = concat2.MapTo<Mp4>(outputSettings).First();

            var metadataInfo1 = MetadataHelpers.GetMetadataInfo(concat2.Command, concat2.StreamIdentifiers.FirstOrDefault());
            var metadataInfo2 = MetadataHelpers.GetMetadataInfo(output.Owner, output.GetStreamIdentifier());

            Assert.True(metadataInfo1.VideoStream.Duration == calculatedDuration);
            Assert.True(metadataInfo2.VideoStream.Duration == TimeSpan.FromSeconds(5));
            Assert.True(metadataInfo2.VideoStream.BitRate == 3000000L);
        }
    }
}
