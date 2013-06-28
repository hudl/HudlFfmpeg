using System;
using System.Collections.Generic;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class Filterchain<TOutput>
        where TOutput : IResource, new()
    {
        public Filterchain()
        {
            Output = new TOutput(); 
            _resources = new List<CommandResourceReceipt>();
        }
        public Filterchain(params IFilter[] filters)
            : this()
        {
            Filters.AddRange(filters); 
        }
        public Filterchain(TOutput outputToUse) 
        {
            Output = outputToUse;
            _resources = new List<CommandResourceReceipt>();
        }
        public Filterchain(TOutput outputToUse, params IFilter[] filters) 
            : this(outputToUse)
        {
            Filters.AddRange(filters); 
        }

        public TOutput Output { get; protected set; }

        private List<CommandResourceReceipt> _resources;
        public IReadOnlyList<CommandResourceReceipt> Resources { get { return _resources.AsReadOnly(); } }

        public AppliesToCollecion<IFilter, TOutput> Filters { get; protected set; }

        public void SetResources(params CommandResourceReceipt[] resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Length == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            SetResources(new List<CommandResourceReceipt>(resources));
        }

        public void SetResources(List<CommandResourceReceipt> resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            _resources = resources;
        }

        public Filterchain<TOutput> Copy()
        {
            return new Filterchain<TOutput>(Filters.List.ToArray());
        }

        public override string ToString()
        {
            if (Filters.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one filter.");
            }
            if (Resources.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            var filterChain = new StringBuilder(100);
            var firstFilter = true;

            _resources.ForEach(resource =>
            {
                filterChain.Append(Formats.Map(resource.Map));
                filterChain.Append(" ");
            });

            Filters.List.ForEach(filter =>
            {
                if (firstFilter)
                {
                    firstFilter = false;
                }
                else
                {
                    filterChain.Append(",");
                }

                filterChain.Append(filter.ToString());
                filterChain.Append(" ");
            });

            filterChain.Append(Formats.Map(Output.Map));

            return filterChain.ToString();
        }
    }
}
