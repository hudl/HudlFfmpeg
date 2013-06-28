using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Filter that applies mixes to audio resources together into a single resource 
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    public class AMix : BaseFilter
    {
        private const int FilterMaxInputs = 4;
        private const string FilterType = "amix";
        private const int AMixInputsDefault = 2;
        private const int AMixDropoutTransitionDefault = 2;

        public AMix() 
            : base(FilterType, FilterMaxInputs)
        {
            Inputs = AMixInputsDefault;
            DropoutTransition = AMixDropoutTransitionDefault;
            Duration = DurationTypes.Longest;
        }

        public int Inputs { get; set; } 
                
        public int DropoutTransition { get; set; }

        public DurationTypes Duration { get; set; }

        public override string ToString() 
        {
            if (Inputs < 2)
            {
                throw new ArgumentException("Number of inputs cannot be less than defualt of 2");
            }
            if (DropoutTransition < 2)
            {
                throw new ArgumentException("Dropout transition cannot be less than default of 2");
            }

            //build the filter string 
            var filter = new StringBuilder(100);
            if (Inputs > 2)
            {
                filter.AppendFormat("{1}inputs={0}", 
                    Inputs, 
                    filter.Length > 0 ? ":" : "=");
            }
            if (Duration != DurationTypes.Longest)  
            {
                filter.AppendFormat("{1}duration={0}", 
                    Duration, 
                    filter.Length > 0 ? ":" : "=");
            }
            if (DropoutTransition > 2)
            {
                filter.AppendFormat("{1}dropout_transition={0}", 
                    DropoutTransition, 
                    filter.Length > 0 ? ":" : "=");
            }

            //return the filter string information 
            return string.Concat(Type, filter.ToString());
        }
    }
}
