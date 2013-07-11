using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class Filterchain<TOutput>
        where TOutput : IResource
    {
        internal Filterchain(TOutput outputToUse) 
        {
            if (outputToUse == null)
            {
                throw new ArgumentNullException("outputToUse");
            }

            ResourceList = new List<CommandResourceReceipt>();
            Output = new FilterchainOutput<TOutput>(this, outputToUse);
            Filters = new AppliesToCollection<IFilter>(outputToUse.GetType());
        }
        internal Filterchain(TOutput outputToUse, params IFilter[] filters) 
            : this(outputToUse)
        {
            if (filters.Length > 0)
            {
                Filters.AddRange(filters); 
            }
        }

        public static implicit operator Filterchain<IResource>(Filterchain<TOutput> filterchain)
        {
            var filterchainNew = new Filterchain<IResource>(filterchain.Output.Resource, filterchain.Filters.List.ToArray());
            if (filterchain.ResourceList.Count > 0)
            {
                filterchainNew.SetResources(filterchain.ResourceList);
            }
            return filterchainNew;
        }

        public AppliesToCollection<IFilter> Filters { get; protected set; }

        public ReadOnlyCollection<CommandResourceReceipt> Resources { get { return ResourceList.AsReadOnly(); } }

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

            ResourceList = resources;
        }

        public Filterchain<TResource> Copy<TResource>()
            where TResource : IResource
        {
            return Filterchain.FilterTo(Output.Resource.Copy<TResource>(), Filters.List.ToArray());
        }

        public FilterchainOutput<TOutput> GetOutput(Command<IResource> command)
        {
            Output.Length = TimeSpan.FromSeconds(Helpers.GetLength(command, this));
            return Output;
        }

        #region Internals
        internal List<CommandResourceReceipt> ResourceList { get; set; } 
        internal FilterchainOutput<TOutput> Output { get; set; }
        #endregion
    }
}
