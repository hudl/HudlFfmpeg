using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// A series of Filterchains that work together to convert Video, Audio, and Image streams.
    /// </summary>
    public class Filtergraph
    {
        public Filtergraph()
        {
            FilterchainList = new List<Filterchain<IResource>>();
        }

        public ReadOnlyCollection<Filterchain<IResource>> Filterchains
        {
            get
            {
                return FilterchainList.AsReadOnly();
            }
        }

        public int Count { get { return FilterchainList.Count; } }

        public bool Contains<TOutput>(Filterchain<TOutput> filterchain)
            where TOutput : IResource
        {
            return FilterchainList.Any(f => f.Output.Resource.Map == filterchain.Output.Resource.Map);
        }

        public int IndexOf<TOutput>(Filterchain<TOutput> filterchain)
            where TOutput : IResource
        {
            return FilterchainList.FindIndex(f => f.Output.Resource.Map == filterchain.Output.Resource.Map);
        }

        /// <summary>
        /// Adds a new instance of a filterchain to the filtergraph
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public Filtergraph FilterTo<TResource>(params IFilter[] filters)
            where TResource : IResource, new()
        {
            var filterchain = Filterchain.FilterTo<TResource>(filters);

            return Add(filterchain);
        }

        /// <summary>
        /// Adds a new instance of a filterchain to the filtergraph
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public Filtergraph FilterTo<TResource>(TResource output, params IFilter[] filters)
            where TResource : IResource
        {
            var filterchain = Filterchain.FilterTo<TResource>(output, filters);

            return Add(filterchain);
        }

        /// <summary>
        /// adds the given Filterchain to the Filtergraph
        /// </summary>
        /// <typeparam name="TOutput">the generic type of the filterchain</typeparam>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Add<TOutput>(Filterchain<TOutput> filterchain)
            where TOutput : IResource
        {
            FilterchainList.Add(filterchain);
            return this;
        }

        /// <summary>
        /// merges the given Filterchain to the Filtergraph
        /// </summary>
        /// <typeparam name="TOutput">the generic type of the filterchain</typeparam>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Merge<TOutput>(Filterchain<TOutput> filterchain, FfmpegMergeOptionType optionType)
            where TOutput : IResource
        {
            var indexOfItem = IndexOf(filterchain); 
            if (indexOfItem != -1 && optionType == FfmpegMergeOptionType.NewWins)
            {
                FilterchainList.RemoveAt(indexOfItem);
                FilterchainList.Insert(indexOfItem, filterchain);
            }
            else if (indexOfItem == -1)
            {
                FilterchainList.Add(filterchain);       
            }

            return this;
        }

        /// <summary>
        /// removes the Filterchain at the given index from the Filtergraph
        /// </summary>
        /// <param name="index">the index of the desired Filterchain to be removed from the Filtergraph</param>
        public Filtergraph RemoveAt(int index)
        {
            FilterchainList.RemoveAt(index);
            return this;
        }
         
        /// <summary>
        /// removes all the Filterchain matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public Filtergraph RemoveAll(Predicate<Filterchain<IResource>> pred)
        {
            FilterchainList.RemoveAll(pred);
            return this; 
        }

        public override string ToString() 
        {
            //perform simple validation on filter graph
            if (FilterchainList.Count == 0)
            {
                throw new InvalidOperationException("Filtergraph must contain at least one Filterchain.");
            }

            var filtergraph = new StringBuilder(100);
            FilterchainList.ForEach(filterchain =>
                {
                    if (filtergraph.Length > 0) filtergraph.Append(";");
                    filtergraph.Append(filterchain.ToString());
                });

            //return the formatted filter command string 
            return filtergraph.ToString();
        }

        #region Internals
        internal List<Filterchain<IResource>> FilterchainList { get; set; }
        #endregion
    }
}
