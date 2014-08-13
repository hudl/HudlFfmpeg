using System;
using System.Text;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Video filter that applies a fade in or out effect.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    public class Fade : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "fade";

         public Fade()
            : base(FilterType, FilterMaxInputs)
        {
        }

        public Fade(double? startUnit, double? lengthInUnits, VideoUnitType unitType)
            : this()
        {
            if (unitType == VideoUnitType.Frames)
            {
                StartFrame = startUnit;
                NumberOfFrames = lengthInUnits;
            }
            else
            {
                StartTime = startUnit;
                Duration = lengthInUnits; 
            }
        }

        public Fade(double? startUnit, double? lengthInUnits, VideoUnitType unitType, FadeTransitionType transitionType)
            : this(startUnit, lengthInUnits, unitType)
        {
            TransitionType = transitionType;
        }

        public FadeTransitionType TransitionType { get; set; }

        public double? StartFrame { get; set; }

        public double? NumberOfFrames { get; set; }

        public double? StartTime { get; set; }

        public double? Duration { get; set; }

        public bool Alpha { get; set; }

        public string Color { get; set; }

        public override void Validate()
        {
            if (StartFrame.HasValue && StartFrame <= 0)
            {
                throw new InvalidOperationException("Start Frame of the Fade must be greater than zero.");
            }

            if (NumberOfFrames.HasValue && NumberOfFrames <= 0)
            {
                throw new InvalidOperationException("Number Of Frames of the Fade must be greater than zero.");
            }

            if (StartTime.HasValue && StartTime <= 0)
            {
                throw new InvalidOperationException("StartTime of the Fade must be greater than zero.");
            }

            if (Duration.HasValue && Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Fade must be greater than zero.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (TransitionType != FadeTransitionType.In)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "t", Formats.EnumValue(TransitionType));
            }

            if (StartFrame.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "s", StartFrame.GetValueOrDefault());
            }

            if (NumberOfFrames.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "n", NumberOfFrames.GetValueOrDefault());
            }

            if (StartTime.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "st", StartTime.GetValueOrDefault());
            }

            if (Duration.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "d", Duration.GetValueOrDefault());
            }

            if (Alpha)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "alpha", 1);
            }

            if (!string.IsNullOrWhiteSpace(Color))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "c", Color);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
