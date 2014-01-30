using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
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
        private const int AMixDropoutTransitionDefault = 2;

        public AMix() 
            : base(FilterType, FilterMaxInputs)
        {
            DropoutTransition = AMixDropoutTransitionDefault;
            DurationType = DurationType.Longest;
        }
        public AMix(DurationType duration)
            : this()
        {
            DurationType = duration;
        }
        public AMix(DurationType duration, int dropoutTransition)
            : this(duration)
        {
            DropoutTransition = dropoutTransition;
        }

        public int DropoutTransition { get; set; }

        public DurationType DurationType { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResourcev2> resources)
        {
            switch (DurationType)
            {
                case DurationType.First:
                    return resources.First().Resource.Length;
                case DurationType.Shortest:
                    return resources.Min(r => r.Resource.Length);
                default:
                    return resources.Max(r => r.Resource.Length);
            }
        }

        public override string ToString() 
        {
            if (CommandResources.Count < 2)
            {
                throw new InvalidOperationException("Number of inputs cannot be less than defualt of 2");
            }
            if (DropoutTransition < 2)
            {
                throw new InvalidOperationException("Dropout transition cannot be less than default of 2");
            }

            //build the filter string 
            var filter = new StringBuilder(100);
            if (CommandResources.Count > 2)
            {
                filter.AppendFormat("{1}inputs={0}",
                    CommandResources.Count, 
                    filter.Length > 0 ? ":" : "=");
            }
            if (DurationType != DurationType.Longest)  
            {
                filter.AppendFormat("{1}duration={0}", 
                    DurationType.ToString().ToLower(), 
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
