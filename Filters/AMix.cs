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
    /// Filter that applies mixes to audio resources together into a single resource 
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    public class AMix : IFilter
    {
        public AMix() 
        {
            Inputs = _inputs;
            DropoutTransition = _dropoutTransition;
        }

        public enum DurationType 
        {
            longest,
            shortest,
            first
        }

        public string Type { get { return "amix" ; } }

        public int MaxInputs { get { return 4; } }
        
        public DurationType Duration { get; set; }

        public int Inputs { get; set; } 
        private int _inputs = 2; 
                
        public int DropoutTransition { get; set; }
        private int _dropoutTransition = 2; 

        public override string ToString() 
        {
            //validate the input first to determine if ( we have anybalancing to do 
            if (Inputs < 2)  
                throw new ArgumentException("Number of inputs cannot be less than defualt of 2", "Inputs");
            if (DropoutTransition < 2)  
                throw new ArgumentException("Dropout transition cannot be less than default of 2", "DropoutTransition");

            //build the filter string 
            StringBuilder filter = new StringBuilder(100);
            if (Inputs > 2)  
                filter.AppendFormat("{1}inputs={0}", 
                    Inputs, 
                    filter.Length > 0 ? ":" : "=");
            if (Duration != DurationType.longest)  
                filter.AppendFormat("{1}duration={0}", 
                    Duration.ToString(), 
                    filter.Length > 0 ? ":" : "=");
            if (DropoutTransition > 2)  
                filter.AppendFormat("{1}dropout_transition={0}", 
                    DropoutTransition, 
                    filter.Length > 0 ? ":" : "=");

            //return the filter string information 
            return string.Concat(Type, filter.ToString());
        }
    }
}
