using System;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
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
            TimebaseUnit = VideoUnitType.Seconds;
        }
        public Trim(double startUnit, double endUnit, double duration, VideoUnitType timebaseUnit)
            : this()
        {
            if (startUnit < 0)
            {
                throw new ArgumentException("Start Unit cannot be less than zero", "startUnit"); 
            }
            if (endUnit < 0)
            {
                throw new ArgumentException("End Unit cannot be less than zero", "endUnit");
            }
            if (endUnit > 0D && endUnit <= startUnit)
            {
                throw new ArgumentException("End Unit cannot be less than Start Unit", "endUnit");
            }
            if (duration <= 0)
            {
                throw new ArgumentException("Duration of trimmed video must be greater than zero", "duration");
            }

            End = endUnit;
            Start = startUnit;
            Duration = duration;
            TimebaseUnit = timebaseUnit;
        }

        /// <summary>
        /// the start measure of where the video is to be trimmed to
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// the end measure of where the video is to be trimmed too
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// the maximum duration of the output (required)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// the base unit of measure for the trim command
        /// </summary>
        public VideoUnitType TimebaseUnit { get; set; }

        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<Command.CommandResource<IResource>> resources)
        {
            return TimeSpan.FromSeconds(Duration);
        }

        public override string ToString() 
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Output duration cannot be empty for a video trim.");
            }

            var filter = new StringBuilder(100);
            switch (TimebaseUnit)
            {
                case VideoUnitType.Frames:
                    if (Start > 0)
                    {
                        filter.AppendFormat("{1}start_frame={0}",
                                Start,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    if (End > 0)
                    {
                        filter.AppendFormat("{1}end_frame={0}",
                                End,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    break;
                default:
                    if (Start > 0)
                    {
                        filter.AppendFormat("{1}start={0}",
                                Start,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    if (End > 0)
                    {
                        filter.AppendFormat("{1}end={0}",
                                End,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    break;
            }

            filter.AppendFormat("{1}duration={0}", 
                    Duration, 
                    (filter.Length > 0) ?  ":" : string.Empty);

            return string.Concat(Type, filter.ToString());
        }
    }
}
