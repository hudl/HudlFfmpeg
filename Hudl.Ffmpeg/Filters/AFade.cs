using System;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Audio filter that applies a fade in or out effect to the audio stream
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    public class AFade : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "afade";

        public AFade()
            : base(FilterType, FilterMaxInputs)
        {
        }

        public AFade(double? startUnit, double? lengthInUnits, AudioUnitType unitType)
            : this()
        {
            if (unitType == AudioUnitType.Sample)
            {
                StartSample = startUnit;
                NumberOfSamples = lengthInUnits;
            }
            else
            {
                StartTime = startUnit;
                Duration = lengthInUnits; 
            }
        }

        public AFade(double? startUnit, double? lengthInUnits, AudioUnitType unitType, FadeTransitionType transitionType)
            : this(startUnit, lengthInUnits, unitType)
        {
            TransitionType = transitionType;
        }

        public AFade(double? startUnit, double? lengthInUnits, AudioUnitType unitType, FadeTransitionType transitionType, FadeCurveType curveType)
            : this(startUnit, lengthInUnits, unitType, transitionType)
        {
            CurveType = curveType; 
        }

        public FadeTransitionType TransitionType { get; set; }

        public FadeCurveType CurveType { get; set; }

        public double? StartSample { get; set; }

        public double? NumberOfSamples { get; set; }

        public double? StartTime { get; set; }

        public double? Duration { get; set; }

        public override void Validate()
        {
            if (StartSample.HasValue && StartSample <= 0)
            {
                throw new InvalidOperationException("Start Sample of the Audio Fade must be greater than zero.");
            }

            if (NumberOfSamples.HasValue && NumberOfSamples <= 0)
            {
                throw new InvalidOperationException("Number Of Samples of the Audio Fade must be greater than zero.");
            }

            if (StartTime.HasValue && StartTime <= 0)
            {
                throw new InvalidOperationException("StartTime of the Audio Fade must be greater than zero.");
            }

            if (Duration.HasValue && Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Audio Fade must be greater than zero.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (TransitionType != FadeTransitionType.In)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "t", Formats.EnumValue(TransitionType));
            }

            if (StartSample.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "ss", StartSample.GetValueOrDefault());
            }

            if (NumberOfSamples.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "ns", NumberOfSamples.GetValueOrDefault());
            }

            if (StartTime.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "st", StartTime.GetValueOrDefault());
            }

            if (Duration.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "d", Duration.GetValueOrDefault());
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
