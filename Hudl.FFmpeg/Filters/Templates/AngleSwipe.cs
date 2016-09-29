using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;


namespace Hudl.FFmpeg.Filters.Templates
{
    public class AngleSwipe : FilterchainTemplate
    {
        private const string SwipeAlgorithm = "if(gt(X+N*{0},Y+W),A,B)";

        public AngleSwipe(TimeSpan crossfadeDuration)
        {
            Duration = crossfadeDuration;
            Speed = 50;
            OverlayDeltaX = 0;
        }

        public AngleSwipe(double crossfadeDuration)
            : this(TimeSpan.FromSeconds(crossfadeDuration))
        {
        }

        public int Speed { get; set; }
        
        public int OverlayDeltaX { get; set; }

        private TimeSpan Duration { get; set; }

        public override List<StreamIdentifier> SetupTemplate(FFmpegCommand command, List<StreamIdentifier> streamIdList)
        {
            if (streamIdList.Count != 3)
            {
                throw new InvalidOperationException("AngleSwipe Concatenate requires two input video streams and an image stream.");
            }

            var streamTo = streamIdList[1];
            var streamFrom = streamIdList[0];
            var streamOverlay = streamIdList[2];

            //grab the current length of the streamId specified 
            var videoMeta = MetadataHelpers.GetMetadataInfo(command, streamFrom).VideoStream.VideoMetadata;

            //from == 
            // - split
            //   - 1: start -> (end - durationOf)
            //   - 2: (end - durationOf) -> end

            //to ==
            // - split
            //   - 1: start -> (start + durationOf)
            //   - 2: (start + durationOf) -> end 

            //blend == 
            // - from:2 / to:1

            //output ==
            // - (from:1, blend, to:2)

            var endMinusDuration = videoMeta.Duration - Duration;

            var fromSplit = command.Select(streamFrom)
                                   .Filter(Filterchain.FilterTo<VideoStream>(new Split(2)));

            var fromMain = fromSplit.Take(0)
                                    .Filter(new TrimVideo(null, endMinusDuration.TotalSeconds, VideoUnitType.Seconds));

            var fromBlend = fromSplit.Take(1)
                                     .Filter(new TrimVideo(endMinusDuration.TotalSeconds, null, VideoUnitType.Seconds));

            var toSplit = command.Select(streamTo)
                                 .Filter(Filterchain.FilterTo<VideoStream>(new Split(2)));

            var toBlend = toSplit.Take(0)
                                 .Filter(new TrimVideo(null, Duration.TotalSeconds, VideoUnitType.Seconds));

            var toMain = toSplit.Take(1)
                                .Filter(new TrimVideo(Duration.TotalSeconds, null, VideoUnitType.Seconds));

            var blendOut = command.Select(toBlend.StreamIdentifiers)
                .Select(fromBlend.StreamIdentifiers)
                .Filter(Filterchain.FilterTo<VideoStream>(new Blend(string.Format(SwipeAlgorithm, Speed))));

            var overlayImage = command.Select(streamOverlay)
                .Filter(Filterchain.FilterTo<VideoStream>(new Scale(videoMeta.Width, videoMeta.Height)));

            var overlay = command.Select(blendOut.StreamIdentifiers)
                .Select(overlayImage.StreamIdentifiers)
                .Filter(Filterchain.FilterTo<VideoStream>(new Overlay($"W-{OverlayDeltaX}-(n+1)*{Speed}", "0")));

            var result = command.Select(fromMain.StreamIdentifiers)
                                .Select(overlay.StreamIdentifiers)
                                .Select(toMain.StreamIdentifiers)
                                .Filter(Filterchain.FilterTo<VideoStream>(new Concat()));

            return result.StreamIdentifiers;
        }
    }
}
