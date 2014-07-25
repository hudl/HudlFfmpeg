using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;

namespace Hudl.FFmpeg.Filters.Templates
{
    public class TrimVideo : FilterchainTemplate
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

        public override List<StreamIdentifier> SetupTemplate(FFmpegCommand command, List<StreamIdentifier> streamIdList)
        {
            if (streamIdList.Count != 1)
            {
                throw new InvalidOperationException("Crossfade Concatenate requires two input video streams.");
            }

            //trim ==
            // - trim filter
            // - reset PTS filter

            var result = command.Select(streamIdList)
                                .Filter(Filterchain.FilterTo<VideoStream>(TrimFilter, new SetPts(SetPtsExpressionType.ResetTimestamp)));

            return result.StreamIdentifiers;
        }
    }
}
