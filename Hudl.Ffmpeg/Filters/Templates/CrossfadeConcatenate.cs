using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;

namespace Hudl.FFmpeg.Filters.Templates
{
    public class CrossfadeConcatenate : FilterchainTemplate
    {
        private const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";

        public CrossfadeConcatenate(TimeSpan crossfadeDuration)
        {
            Duration = crossfadeDuration; 
        }
        public CrossfadeConcatenate(double crossfadeDuration)
        {
            Duration = TimeSpan.FromSeconds(crossfadeDuration); 
        }

        private TimeSpan Duration { get; set; }

        public override List<StreamIdentifier> SetupTemplate(FFmpegCommand command, List<StreamIdentifier> streamIdList)
        {
            if (streamIdList.Count != 2)
            {
                throw new InvalidOperationException("Crossfade Concatenate requires two input video streams.");
            }

            var streamTo = streamIdList[1];
            var streamFrom = streamIdList[0];

            //grab the current length of the streamId specified 
            var streamFromMetadata = MetadataHelpers.GetMetadataInfo(command, streamFrom);

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

            var endMinusDuration = streamFromMetadata.VideoStream.VideoMetadata.Duration - Duration;

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
                                  .Filter(Filterchain.FilterTo<VideoStream>(new Blend(string.Format(CrossfadeAlgorithm, Duration.TotalSeconds))));

            var result = command.Select(fromMain.StreamIdentifiers)
                                .Select(blendOut.StreamIdentifiers)
                                .Select(toMain.StreamIdentifiers)
                                .Filter(Filterchain.FilterTo<VideoStream>(new Concat()));

            return result.StreamIdentifiers;
        }
    }
}
