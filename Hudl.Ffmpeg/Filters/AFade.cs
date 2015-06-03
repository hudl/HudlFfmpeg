using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Audio filter that applies a fade in or out effect to the audio stream
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    [Filter(Name = "afade", MinInputs = 1, MaxInputs = 1)]
    public class AFade : IFilter
    {
        public AFade()
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

        [FilterParameter(Name = "t", Default = FadeTransitionType.In, Formatter = typeof(EnumParameterFormatter))]
        public FadeTransitionType TransitionType { get; set; }

        [FilterParameter(Name = "curve", Default = FadeCurveType.Tri, Formatter = typeof(EnumParameterFormatter))]
        public FadeCurveType CurveType { get; set; }

        [FilterParameter(Name = "ss")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double? StartSample { get; set; }

        [FilterParameter(Name = "ns")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double? NumberOfSamples { get; set; }

        [FilterParameter(Name = "st")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double? StartTime { get; set; }

        [FilterParameter(Name = "d")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double? Duration { get; set; }
    }
}
