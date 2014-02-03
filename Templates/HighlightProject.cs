using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    public class HighlightProject : BaseCommandFactoryTemplate
    {
        public HighlightProject(CommandConfiguration configuration)
            : base(configuration)
        {
            Flags = FlagTypes.None;
            CommandData = new List<VideoCommandData>();
        }

        [Flags]
        public enum FlagTypes
        {
            None = 0x0,
            OutputHd = 0x1,
            OutputSd = 0x2
        }

        public FlagTypes Flags { get; set; }

        public List<VideoCommandData> CommandData { get; set; } 

        private bool HasFlag(FlagTypes flag)
        {
            return (Flags & flag) == flag; 
        }

        protected override void SetupTemplate()
        {
            if (CommandData.Count == 0)
            {
                throw new InvalidOperationException("Cannot create a highlight project with an empty video list.");
            }

            //settings definitiions
            #region ...
            var resolutionTemplateHd = Resolution720P.Create<Mp4>();
            var resolutionTemplateSd = Resolution480P.Create<Mp4>();
            var stepOneOutputSettingsHd = SettingsCollection.ForOutput(
                new Level(3.0),
                new FrameRate(29.97),
                new OverwriteOutput(),
                new BitRateVideo(3000),
                new BitRateCompatibility(3000),
                new CodecVideo(VideoCodecType.Libx264),
                new PixelFormat(PixelFormatType.Yuv420P),
                new ProfileVideo(VideoProfileType.Baseline)
            );
            var stepOneOutputSettingsSd = SettingsCollection.ForOutput(
                new Level(3.0),
                new FrameRate(29.97),
                new OverwriteOutput(),
                new BitRateVideo(1100),
                new BitRateCompatibility(1100),
                new CodecVideo(VideoCodecType.Libx264),
                new PixelFormat(PixelFormatType.Yuv420P),
                new ProfileVideo(VideoProfileType.Baseline)
            );
            stepOneOutputSettingsHd.MergeRange(resolutionTemplateHd, FfmpegMergeOptionType.NewWins);
            stepOneOutputSettingsSd.MergeRange(resolutionTemplateSd, FfmpegMergeOptionType.NewWins);
            #endregion 

            CommandData.ForEach(video =>
                {
                    var command = Factory.AsOutput()
                                         .WithInput(video.FullName);
                   
                    var receipt = ApplyTrimFilterchain(command, video);

                    var splits = ApplySplitStreamFilterchain(command, receipt); 

                    if (HasFlag(FlagTypes.OutputSd))
                    {
                        var outputSd = ApplySdResizeFilterchain(command, splits.ElementAt(0));

                        command.WithReceipts(outputSd)
                               .MapTo<Mp4>(stepOneOutputSettingsSd); 
                    }

                    if (HasFlag(FlagTypes.OutputHd))
                    {
                        var outputHd = ApplyHdResizeFilterchain(command, splits.ElementAt(1)); 

                        command.WithReceipts(outputHd)
                               .MapTo<Mp4>(stepOneOutputSettingsHd); 
                    }
                });
        }

        private CommandReceipt ApplyTrimFilterchain(Commandv2 command,  VideoCommandData commandData)
        {
            var inputReceipt = command.ResourceReceiptAt(0); 

            if (commandData.StartPointInTicks <= 0 || commandData.StopPointInTicks <= 0)
            {
                return inputReceipt;
            }

            var filterchain = Filterchain.FilterTo<Mp4>(1, new Trim(), new SetPts(true)); 
            if (commandData.StartPointInTicks > 0)
            {
                filterchain.Filters.Get<Trim>().Start = TimeSpan.FromTicks(commandData.StartPointInTicks).TotalSeconds; 
            }
            if (commandData.StopPointInTicks > 0)
            {
                filterchain.Filters.Get<Trim>().End = TimeSpan.FromTicks(commandData.StopPointInTicks).TotalSeconds; 

            }

            return command.WithReceipts(inputReceipt)
                          .Filter(filterchain)
                          .Receipts.First(); 
        }

        private List<CommandReceipt> ApplySplitStreamFilterchain(Commandv2 command, CommandReceipt receipt)
        {
            var renderInHd = HasFlag(FlagTypes.OutputHd);
            var renderInSd = HasFlag(FlagTypes.OutputSd);
            if (renderInSd && renderInHd)
            {
                return command.WithReceipts(receipt)
                              .Filter(Filterchain.FilterTo<Mp4>(1, new Split(2)))
                              .Receipts;
            }

            return new List<CommandReceipt> { receipt, receipt };
        }  

        private CommandReceipt ApplyHdResizeFilterchain(Commandv2 command, CommandReceipt receipt)
        {
            const long expectedBitRate = 3000L;
            const int expectedHeight = 720;
            const int expectedWidth = 1280;

            return ApplyResizeFilterchain(command, receipt, ScalePresetType.Hd720, expectedBitRate, expectedWidth, expectedHeight); 
        }
        private CommandReceipt ApplySdResizeFilterchain(Commandv2 command, CommandReceipt receipt)
        {
            const long expectedBitRate = 1100L;
            const int expectedHeight = 480;
            const int expectedWidth = 852;

            return ApplyResizeFilterchain(command, receipt, ScalePresetType.Hd720, expectedBitRate, expectedWidth, expectedHeight);
        }
        private static CommandReceipt ApplyResizeFilterchain(Commandv2 command, CommandReceipt receipt, ScalePresetType type,
                                                      long expectedBitRate, int expectedWidth, int expectedHeight)
        {
            var resource = command.Resources.First().Resource;

            if (resource.Info.BitRate == expectedBitRate &&
                resource.Info.Dimensions.Width == expectedWidth &&
                resource.Info.Dimensions.Height == expectedHeight)
            {
                return receipt;
            }

            var filterchain = Filterchain.FilterTo<Mp4>(1,
                new Pad(Pad.ExprConvertTo169Aspect),
                new Scale(type),
                new SetDar(FfmpegRatio.Create(16, 9)),
                new SetSar(FfmpegRatio.Create(1, 1))
            );

            return command.WithReceipts(receipt)
                          .Filter(filterchain)
                          .Receipts.First(); 
        }
    }

    public class VideoCommandData
    {
        private VideoCommandData()
        {
        }

        public string FullName { get; internal set; }

        public long StartPointInTicks { get; internal set; }

        public long StopPointInTicks { get; internal set; }

        public static VideoCommandData Create(string fullName, long startPointInTicks, long stopPointInTicks)
        {
            return new VideoCommandData
                {
                    FullName = fullName,
                    StopPointInTicks = stopPointInTicks,
                    StartPointInTicks = startPointInTicks,
                };
        }
    }
}
 