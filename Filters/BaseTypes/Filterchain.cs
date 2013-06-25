using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    class Filterchain<Output>
        where Output : IResource, new()
    {
        private new Output _output; 
        private new AppliesToCollecion<IFilter, Output> _filters;

        public Filterchain()
        {
            _output = new Output(); 
        }
        public Filterchain(params IFilter[] filters) 
            : this()
        {
            Filters.AddRange(filters); 
        }
        public Filterchain(Output outputToUse) 
        {
            _output = outputToUse;
        }
        public Filterchain(Output outputToUse, params IFilter[] filters)
            : this(outputToUse)
        {
            Filters.AddRange(filters); 
        }

        public readonly Output Output { get { return _output; } }

        public readonly AppliesToCollecion<IFilter, Output> Filters { get { return _filters; } }

        public override string ToString() 
        {
            //perform simple validation on the filter chain
            if (Filters.Count == 0)
                throw new ArgumentException("Filterchain must contain at least one filter.");

            //process all the filters in the filter chain
            StringBuilder filterChain = new StringBuilder(100); 
            foreach(var filter in Filters.Items) 
            {
                if (filterChain.Length > 0) filterChain.Append(",");
                filterChain.Append(filter.ToString())
                           .Append(" "); 
            }

            //return the filter chain command
            return filterChain.ToString();
        }
    }
}
