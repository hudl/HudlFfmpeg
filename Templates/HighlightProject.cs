using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    public class HighlightProject : BaseCommandFactoryTemplate
    {
        public const int NumberInSegments = 4;
        public const string FfmpegStepOneOutputPrefix = "ffmpeg_1_";

        public HighlightProject(CommandConfiguration configuration)
            : base(configuration)
        {
        }
        
        public 

        protected override void SetupTemplate()
        {
            if (VideoList.Count == 0)
            {
                throw new InvalidOperationException("Cannot create a highlight project with an empty video list.");
            }

            //As of 1-30-2014 the highlight project will be used for trimming only, 
            //we will process 4 videos at a time. 
            var videoListSegments = SegmentList(VideoList, NumberInSegments);

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

            //filter definitiions
            #region ...
            var stepOne = Filterchain.FilterTo<Mp4>(
                new Overlay()
            );
            #endregion 

            videoListSegments.ForEach(videoInput =>
                {
                    var videoOutput = videoInput.Select(video =>
                        {
                            var temporaryInfo = new FileInfo(video);
                            var temporaryName = string.Concat(FfmpegStepOneOutputPrefix, temporaryInfo.Name);
                            return temporaryInfo.FullName.Replace(temporaryInfo.Name, temporaryName); 
                        }).ToList();

                    var command = Factory.CreateOutput()
                                         .UsingVideo(videoInput)
                                         .GivingVideo(videoOutput); 

                    var streamModifier = command.StreamAt(0)
                                                .FilterTo( )

                    
                });


        }

        private static List<List<string>> SegmentList(List<string> listToSegment, int numberInSegment)
        {
            var segmentList = new List<string>();
            var segmentedList = new List<List<string>>();
            var segmentCounter = 0;
            listToSegment.ForEach(item =>
                {
                    segmentCounter++; 

                    if (segmentCounter > 4)
                    {
                        segmentedList.Add(segmentList);

                        segmentList = new List<string>();
                    }

                    segmentList.Add(item);
                });

            if (segmentList.Count > 0)
            {
                segmentedList.Add(segmentList);
            }

            return segmentedList;
        }
    }
}
 