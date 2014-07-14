using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Sugar;

namespace Hudl.Ffmpeg.Filters.Templates
{
    public class TrimVideo<TResource> : FilterchainTemplate
        where TResource : IResource, new()
    {
        public TrimVideo(double? startUnit, double? endUnit, VideoUnitType timebaseUnit)
        {
            TrimFilter = new Trim(startUnit, endUnit, timebaseUnit);
        }
        public TrimVideo(double? startUnit, double? endUnit, double? duration, VideoUnitType timebaseUnit)
        {
            TrimFilter = new Trim(startUnit, endUnit, duration, timebaseUnit);
        }

        private Trim TrimFilter { get; set; }

        public override List<CommandReceipt> SetupTemplate(FfmpegCommand command, List<CommandReceipt> receiptList)
        {
            if (receiptList.Count != 1)
            {
                throw new InvalidOperationException("Crossfade Concatenate requires two input video streams.");
            }

            //trim ==
            // - trim filter
            // - reset PTS filter

            var result = command.WithStreams(receiptList)
                                .Filter(Filterchain.FilterTo<TResource>(TrimFilter, new SetPts(SetPtsExpressionType.ResetTimestamp)));

            return result.Receipts;
        }
    }
}
