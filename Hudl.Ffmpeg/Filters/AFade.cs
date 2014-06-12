using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Audio filter that applies a fade in or out effect.
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class AFade : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "afade";

        public AFade()
            : base(FilterType, FilterMaxInputs)
        {
            Transition = FadeTransitionType.In;
            Unit = AudioUnitType.Seconds;
        }
        public AFade(FadeTransitionType transition, double duration)
            : this()
        {
            Transition = transition;
            Duration = duration;
        }
        public AFade(FadeTransitionType transition, double duration, double overrideStartAt)
            : this(transition, duration)
        {
            OverrideStartAt = overrideStartAt;
        }

        public FadeTransitionType Transition { get; set; } 

        public AudioUnitType Unit { get; set; }
        
        public double Duration { get; set; }

        public double? OverrideStartAt { get; set; }

        public override void Validate()
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Audio Fade must be greater than zero.");
            }
        }

        public override string ToString()
        {
            var filter = new StringBuilder(100);
            var startAtLocation = 0d;
            if (OverrideStartAt.HasValue)
            {
                startAtLocation = OverrideStartAt.Value;
            }
            else if (Transition == FadeTransitionType.Out)
            {
                startAtLocation = CommandResources[0].Resource.Info.Duration.TotalSeconds - Duration; 
            }
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit) 
            {
                case AudioUnitType.Sample:
                    filter.AppendFormat(":ss={0}:ns={1}",
                        startAtLocation, 
                        Duration);
                    break;
                default : //seconds 
                    filter.AppendFormat(":st={0}:d={1}",
                        startAtLocation, 
                        Duration);
                    break;
            }
 
            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
