using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;

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
        public AFade(FadeTransitionTypes transition, int duration)
            : this()
        {
            Transition = transition;
            Duration = duration;
        }

        public FadeTransitionTypes Transition { get; set; } 

        public FadeAudioUnitTypes Unit { get; set; }
        
        public double Duration { get; set; }

        public override string ToString()
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Audio Fade must be greater than zero.");
            }

            var filter = new StringBuilder(100);
            var startAtLocation = 0d;
            if (Transition == FadeTransitionTypes.Out)
            {
                startAtLocation = Resources[0].Resource.Length.TotalSeconds - Duration; 
            }
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit) 
            {
                case FadeAudioUnitTypes.Sample:
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
