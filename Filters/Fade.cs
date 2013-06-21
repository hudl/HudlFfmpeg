using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    [AppliesToResource(Type=typeof(IVideo))]
    public class Fade : IFilter
    {
        /// <summary>
        /// the video fade type that is to be applied
        /// </summary>
        public enum FadeType 
        {
            @in,
            @out
        }

        /// <summary>
        /// the fade amount unit of measurement
        /// </summary>
        public enum FadeUnits 
        {
            seconds,
            frames
        }

        public FadeUnits Unit { get; set; }

        public FadeType Transition { get; set; } 

        public double StartAt { get; set; } 

        public double Duration { get; set; } 

        public string Type { get { return "fade"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString()
        {
            if (StartAt == null)
                throw new ArgumentNullException("Starting location of the Video Fade cannot be null.", "StartAt");
            if (Duration == null)
                throw new ArgumentNullException("Duration of the Video Fade cannot be null.", "Duration");

            StringBuilder filter = new StringBuilder(100);
            filter.AppendFormat("t={0}", Transition.ToString());
            switch (Unit)
            {
                case FadeUnits.frames:
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
