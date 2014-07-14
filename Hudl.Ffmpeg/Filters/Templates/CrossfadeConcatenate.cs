using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Sugar;

namespace Hudl.Ffmpeg.Filters.Templates
{
    public class CrossfadeConcatenate<TResource> : FilterchainTemplate
        where TResource : IResource, new()
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

        public override List<CommandReceipt> SetupTemplate(FfmpegCommand command, List<CommandReceipt> receiptList)
        {
            if (receiptList.Count != 2)
            {
                throw new InvalidOperationException("Crossfade Concatenate requires two input video streams.");
            }

            var streamTo = receiptList[1];
            var streamFrom = receiptList[0];

            //grab the current length of the receipt specified 
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

            var endMinusDuration = streamFromMetadata.Duration - Duration;

            var fromSplit = command.WithStreams(streamFrom)
                                   .Filter(Filterchain.FilterTo<TResource>(new Split(2)));

            var fromMain = fromSplit.TakeStreamAt(0)
                                    .Filter(new TrimVideo<TResource>(null, endMinusDuration.TotalSeconds, VideoUnitType.Seconds));

            var fromBlend = fromSplit.TakeStreamAt(1)
                                     .Filter(new TrimVideo<TResource>(endMinusDuration.TotalSeconds, null, VideoUnitType.Seconds));

            var toSplit = command.WithStreams(streamTo)
                                 .Filter(Filterchain.FilterTo<TResource>(new Split(2)));

            var toBlend = toSplit.TakeStreamAt(0)
                                 .Filter(new TrimVideo<TResource>(null, Duration.TotalSeconds, VideoUnitType.Seconds));

            var toMain = toSplit.TakeStreamAt(1)
                                .Filter(new TrimVideo<TResource>(Duration.TotalSeconds, null, VideoUnitType.Seconds));

            var blendOut = command.WithStreams(fromBlend.Receipts)
                                  .WithStreams(toBlend.Receipts)
                                  .Filter(Filterchain.FilterTo<TResource>(new Blend(CrossfadeAlgorithm)));

            var result = command.WithStreams(fromMain.Receipts)
                                .WithStreams(blendOut.Receipts)
                                .WithStreams(toMain.Receipts)
                                .Filter(Filterchain.FilterTo<TResource>(new Concat()));

            return result.Receipts;
        }
    }
}
