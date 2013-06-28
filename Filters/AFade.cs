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
            Transition = FadeTransitionTypes.In;
            Unit = FadeAudioUnitTypes.Seconds;
        }
        public AFade(FadeTransitionTypes transition, int startAt, int duration)
            : this()
        {
            Transition = transition;
            Duration = duration;
            StartAt = startAt;
        }

        public FadeTransitionTypes Transition { get; set; } 

        public FadeAudioUnitTypes Unit { get; set; }
        
        public double StartAt { get; set; }
        
        public double Duration { get; set; }

        public override string ToString()
        {
            if (StartAt == 0)
            {
                throw new ArgumentException("Starting location of the Audio Fade cannot be null.", "StartAt");
            }
            if (Duration == 0)
            {
                throw new ArgumentException("Duration of the Audio Fade cannot be null.", "Duration");
            }

            var filter = new StringBuilder(100);
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit) 
            {
                case FadeUnits.Sample:
                    filter.AppendFormat(":ss={0}:ns={1}", 
                        StartAt, 
                        Duration);
                    break;
                default : //seconds 
                    filter.AppendFormat(":st={0}:d={1}", 
                        StartAt, 
                        Duration);
                    break;
            }
 
            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
