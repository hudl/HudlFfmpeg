using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Video filter that applies a fade in or out effect.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class Fade : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "fade";

        public Fade()
            : base(FilterType, FilterMaxInputs)
        {
            Transition = FadeTransitionTypes.In;
            Unit = FadeVideoUnitTypes.Seconds;
        }
        public Fade(FadeTransitionTypes transition, int duration)
            : this() 
        {
            Transition = transition;
            Duration = duration;
        }

        public int Duration { get; set; } 

        public FadeVideoUnitTypes Unit { get; set; }

        public FadeTransitionTypes Transition { get; set; }

        public override string ToString()
        {
            if (Duration == 0)
            {
                throw new InvalidOperationException("Duration of the Video Fade cannot be zero.");
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
                case FadeVideoUnitTypes.Frames:
                    filter.AppendFormat(":s={0}:n={1}",
                        startAtLocation,
                        Duration);
                    break;
                default: //seconds 
                    filter.AppendFormat(":st={0}:d={1}",
                        startAtLocation,
                        Duration);
                    break;
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
