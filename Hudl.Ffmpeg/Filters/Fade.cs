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
            Transition = FadeTransitionType.In;
            Unit = VideoUnitType.Seconds;
        }
        public Fade(FadeTransitionType transition, double duration)
            : this() 
        {
            Transition = transition;
            Duration = duration;
        }
        public Fade(FadeTransitionType transition, double duration, double overrideStartAt)
            : this(transition, duration)
        {
            OverrideStartAt = overrideStartAt;
        }

        public double Duration { get; set; }

        public double? OverrideStartAt { get; set; }

        public VideoUnitType Unit { get; set; }

        public FadeTransitionType Transition { get; set; }

        public override string ToString()
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Video Fade cannot be zero.");
            }

            var filter = new StringBuilder(100);
            var startAtLocation = 0d;
            if (Transition == FadeTransitionType.Out)
            {
                startAtLocation = CommandResources[0].Resource.Length.TotalSeconds - Duration;
            }
            if (OverrideStartAt.HasValue)
            {
                startAtLocation = OverrideStartAt.Value;
            }
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit)
            {
                case VideoUnitType.Frames:
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
