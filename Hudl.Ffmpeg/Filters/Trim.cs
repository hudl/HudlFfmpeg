using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Trim Video filter trims down the length of a video to within the constraints provided.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "trim", MinInputs = 1, MaxInputs = 1)]
    public class Trim : IFilter, IMetadataManipulation
    {
        public Trim() 
        {
        }
        public Trim(double? startUnit, double? endUnit, VideoUnitType timebaseUnit)
            : this()
        {
            switch (timebaseUnit)
            {
                case VideoUnitType.Frames:
                    EndFrame = endUnit;
                    StartFrame = startUnit;
                    break;
                case VideoUnitType.Seconds:
                    End = endUnit;
                    Start = startUnit;
                    break;
                case VideoUnitType.Timebase:
                    EndPts = endUnit;
                    StartPts = startUnit;
                    break;
            }
        }
        public Trim(double? startUnit, double? endUnit, double? duration, VideoUnitType timebaseUnit)
            : this(startUnit, endUnit, timebaseUnit)
        {
            Duration = duration;
        }

        [FilterParameter(Name = "start")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? Start { get; set; }

        [FilterParameter(Name = "end")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? End { get; set; }

        [FilterParameter(Name = "start_pts")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? StartPts { get; set; }

        [FilterParameter(Name = "end_pts")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? EndPts { get; set; }

        [FilterParameter(Name = "start_frame")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? StartFrame { get; set; }

        [FilterParameter(Name = "end_frame")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? EndFrame { get; set; }

        [FilterParameter(Name = "duration")]
        [Validator(LogicalOperators.GreaterThan, 0)]
        public double? Duration { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            var startTimeInSeconds = 0D;
            var endTimeInSeconds = infoToUpdate.VideoStream.VideoMetadata.Duration.TotalSeconds;

            if (End.HasValue)
            {
                endTimeInSeconds = End.Value;
            } 
            else if (EndFrame.HasValue)
            {
                endTimeInSeconds = (double)EndFrame.Value / (double)infoToUpdate.VideoStream.VideoMetadata.AverageFrameRate.ToDouble();
            }
            else if (EndPts.HasValue)
            {
                endTimeInSeconds = (double)EndPts.Value / (double)infoToUpdate.VideoStream.VideoMetadata.TimeBase.ToDouble();
            }

            if (Start.HasValue)
            {
                startTimeInSeconds = Start.Value;
            }
            else if (StartFrame.HasValue)
            {
                startTimeInSeconds = (double)StartFrame.Value / (double)infoToUpdate.VideoStream.VideoMetadata.AverageFrameRate.ToDouble();
            }
            else if (StartPts.HasValue)
            {
                startTimeInSeconds = (double)StartPts.Value / (double)infoToUpdate.VideoStream.VideoMetadata.TimeBase.ToDouble();
            }

            var timeInSecondsAfterTrim = endTimeInSeconds - startTimeInSeconds;
            if (timeInSecondsAfterTrim < 0)
            {
                timeInSecondsAfterTrim = 0;
            }

            infoToUpdate.VideoStream.VideoMetadata.Duration = TimeSpan.FromSeconds(timeInSecondsAfterTrim);

            return infoToUpdate;
        }
    }
}
