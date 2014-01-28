using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// A series of Filterchains that work together to convert Video, Audio, and Image streams.
    /// </summary>
    public class Filtergraphv2
    {
        private Filtergraphv2()
        {
            FilterchainList = new List<Filterchainv2>();
        }

        public static Filtergraphv2 Create(Commandv2 command)
        {
            return new Filtergraphv2
                {
                    Owner = command
                };
        }

        public ReadOnlyCollection<Filterchainv2> Filterchains
        {
            get
            {
                return FilterchainList.AsReadOnly();
            }
        }

        public int Count { get { return FilterchainList.Count; } }

        public bool Contains(Filterchainv2 filterchain)
        {
            return FilterchainList.Any(f => f.Id == filterchain.Id); 
        }

        public int IndexOf(Filterchainv2 filterchain)
        {
            return FilterchainList.FindIndex(f => f.Id == filterchain.Id);
        }

        /// <summary>
        /// Adds a new instance of a filterchain to the filtergraph
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public Filtergraphv2 FilterTo<TResource>(int count, params IFilter[] filters)
            where TResource : IResource, new()
        {
            var filterchain = Filterchain.FilterTo<TResource>(count, filters);

            return Add(filterchain);
        }

        /// <summary>
        /// Adds a new instance of a filterchain to the filtergraph
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public Filtergraphv2 FilterTo(List<IResource> outputList, params IFilter[] filters)
        {
            var filterchain = Filterchain.FilterTo(outputList, filters);

            return Add(filterchain);
        }

        /// <summary>
        /// adds the given Filterchain to the Filtergraph
        /// </summary>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraphv2 Add(Filterchainv2 filterchain)
        {
            FilterchainList.Add(filterchain);
            return this;
        }

        /// <summary>
        /// merges the given Filterchain to the Filtergraph
        /// </summary>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraphv2 Merge(Filterchainv2 filterchain, FfmpegMergeOptionType optionType)
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
        public Filtergraphv2 RemoveAt(int index)
        {
            FilterchainList.RemoveAt(index);
            return this;
        }
         
        /// <summary>
        /// removes all the Filterchain matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public Filtergraphv2 RemoveAll(Predicate<Filterchainv2> pred)
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
        internal Commandv2 Owner { get; set; }
        internal List<Filterchainv2> FilterchainList { get; set; }
        #endregion
    }
}
