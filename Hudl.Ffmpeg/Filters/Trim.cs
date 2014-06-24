using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Trim Video filter trims down the length of a video to within the constraints provided.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class Trim : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "trim";

        public Trim() 
            : base(FilterType, FilterMaxInputs)
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

        public double? Start { get; set; }

        public double? End { get; set; }

        public double? StartPts { get; set; }

        public double? EndPts { get; set; }

        public double? StartFrame { get; set; }

        public double? EndFrame { get; set; }

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
            if (StartFrame.HasValue && StartFrame <= 0)
            {
                throw new InvalidOperationException("StartFrame must be a value greater than zero.");
            }
            if (EndFrame.HasValue && EndFrame <= 0)
            {
                throw new InvalidOperationException("EndFrame must be a value greater than zero.");
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
            if (StartFrame.HasValue && StartFrame > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "start_frame", StartFrame);
            }
            if (EndFrame.HasValue && EndFrame > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "end_frame", EndFrame);
            }
            if (Duration.HasValue && Duration > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "duration", Duration);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
        
        //TODO: legacy 
        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<CommandResource> resources)
        {
            return TimeSpan.FromSeconds(Duration ?? 0D);
        }
    }
}
