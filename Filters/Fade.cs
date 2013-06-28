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
        public Fade(FadeTransitionTypes transition, int startAt, int duration)
            : this() 
        {
            Transition = transition;
            Duration = duration;
            StartAt = startAt; 
        }

        public int StartAt { get; set; }

        public int Duration { get; set; } 

        public FadeVideoUnitTypes Unit { get; set; }

        public FadeTransitionTypes Transition { get; set; }

        public override string ToString()
        {
            if (StartAt == 0)
            {
                throw new ArgumentException("Starting location of the Video Fade cannot be zero.");
            }
            if (Duration == 0)
            {
                throw new ArgumentException("Duration of the Video Fade cannot be zero.");
            }

            var filter = new StringBuilder(100);
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit)
            {
                case FadeVideoUnitTypes.Frames:
                    filter.AppendFormat(":s={0}:n={1}",
                        StartAt,
                        Duration);
                    break;
                default: //seconds 
                    filter.AppendFormat(":st={0}:d={1}",
                        StartAt,
                        Duration);
                    break;
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
