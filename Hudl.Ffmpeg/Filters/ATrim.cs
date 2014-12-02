using System;
using System.Collections.Generic;
using System.Text;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Trim Video filter trims down the length of a video to within the constraints provided.
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    public class ATrim : BaseFilter, IMetadataManipulation
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "atrim";

        public ATrim() 
            : base(FilterType, FilterMaxInputs)
        {
        }
        public ATrim(double? startUnit, double? endUnit, AudioUnitType timebaseUnit)
            : this()
        {
            switch (timebaseUnit)
            {
                case AudioUnitType.Sample:
                    EndSample = endUnit;
                    StartSample = startUnit;
                    break;
                case AudioUnitType.Seconds:
                    End = endUnit;
                    Start = startUnit;
                    break;
            }
        }

        public double? Start { get; set; }

        public double? End { get; set; }

        public double? StartPts { get; set; }

        public double? EndPts { get; set; }

        public double? StartSample { get; set; }

        public double? EndSample { get; set; }

        public double? Duration { get; set; }

        public override void Validate()
        {
            if (Start.HasValue && Start <= 0)
            {
                throw new InvalidOperationException("Start must be a value greater than zero.");
            }
            if (End.HasValue && End <= 0)
            {
                throw new InvalidOperationException("End must be a value greater than zero.");
            }
            if (StartPts.HasValue && StartPts <= 0)
            {
                throw new InvalidOperationException("StartPts must be a value greater than zero.");
            }
            if (EndPts.HasValue && EndPts <= 0)
            {
                throw new InvalidOperationException("EndPts must be a value greater than zero.");
            }
            if (StartSample.HasValue && StartSample <= 0)
            {
                throw new InvalidOperationException("StartSample must be a value greater than zero.");
            }
            if (EndSample.HasValue && EndSample <= 0)
            {
                throw new InvalidOperationException("EndSample must be a value greater than zero.");
            }
            if (Duration.HasValue && Duration <= 0)
            {
                throw new InvalidOperationException("Duration must be a value greater than zero.");
            }
        }

        public override string ToString() 
        {
            var filterParameters = new StringBuilder(100);

            if (Start.HasValue && Start > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "start", Start);
            }
            if (End.HasValue && End > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "end", End);
            }
            if (StartPts.HasValue && StartPts > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "start_pts", StartPts);
            }
            if (EndPts.HasValue && EndPts > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "end_pts", EndPts);
            }
            if (StartSample.HasValue && StartSample > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "start_sample", StartSample);
            }
            if (EndSample.HasValue && EndSample > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "end_sample", EndSample);
            }
            if (Duration.HasValue && Duration > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "duration", Duration);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            var startTimeInSeconds = 0D;
            var endTimeInSeconds = infoToUpdate.VideoStream.Duration.TotalSeconds;

            if (End.HasValue)
            {
                endTimeInSeconds = End.Value;
            } 
            else if (EndSample.HasValue)
            {
                endTimeInSeconds = (double)EndSample.Value / (double)infoToUpdate.AudioStream.FrameRate.ToDouble();
            }
            else if (EndPts.HasValue)
            {
                endTimeInSeconds = (double)EndPts.Value / (double)infoToUpdate.AudioStream.Timebase.ToDouble();
            }

            if (Start.HasValue)
            {
                startTimeInSeconds = Start.Value;
            }
            else if (StartSample.HasValue)
            {
                startTimeInSeconds = (double)StartSample.Value / (double)infoToUpdate.VideoStream.FrameRate.ToDouble();
            }
            else if (StartPts.HasValue)
            {
                startTimeInSeconds = (double)StartPts.Value / (double)infoToUpdate.VideoStream.Timebase.ToDouble();
            }

            var timeInSecondsAfterTrim = endTimeInSeconds - startTimeInSeconds;
            if (timeInSecondsAfterTrim < 0)
            {
                timeInSecondsAfterTrim = 0;
            }

            infoToUpdate.VideoStream.Duration = TimeSpan.FromSeconds(timeInSecondsAfterTrim);

            return infoToUpdate;
        }
    }
}
