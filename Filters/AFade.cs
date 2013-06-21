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
    /// <summary>
    /// Filter that applies a fade in or out effect on audio resources
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class AFade : IFilter
    {
        /// <summary>
        /// the audio fade type that is to be applied
        /// </summary>
        enum FadeType 
        {
            @in,
            @out
        }

        /// <summary>
        /// the fade amount unit of measurement
        /// </summary>
        enum FadeUnits 
        {
            seconds,
            sample
        }

        public string Type { get { return "afade"; } }

        public int MaxInputs { get { return 1; } }

        public FadeType Transition { get; set; } 

        public FadeUnits Unit { get; set; }
        
        public double StartAt { get; set; }
        
        public double Duration { get; set; }

        public override string ToString() 
        {
            if (StartAt == null) 
                throw new ArgumentNullException("Starting location of the Audio Fade cannot be null.", "StartAt");
            if (Duration == null)
                throw new ArgumentNullException("Duration of the Audio Fade cannot be null.", "Duration");

            StringBuilder filter = new StringBuilder(100);
            filter.AppendFormat("t={0}", Transition.ToString());
            switch (Unit) 
            {
                case FadeUnits.sample:
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
 
            return String.Concat(Type, "=", filter.ToString());
        }
    }
}
