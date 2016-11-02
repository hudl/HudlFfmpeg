using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    /// <summary>
    /// A series of Filterchains that work together to convert Video, Audio, and Image streams.
    /// </summary>
    public class Filtergraph
    {
        private Filtergraph()
        {
            FilterchainList = new List<Filterchain>();
        }

        public static Filtergraph Create(FFmpegCommand command)
        {
            return new Filtergraph
                {
                    Owner = command
                };
        }

        public ReadOnlyCollection<Filterchain> Filterchains
        {
            get
            {
                return FilterchainList.AsReadOnly();
            }
        }

        public int Count { get { return FilterchainList.Count; } }

        public bool Contains(Filterchain filterchain)
        {
            return FilterchainList.Any(f => f.Id == filterchain.Id); 
        }

        public int IndexOf(Filterchain filterchain)
        {
            return FilterchainList.FindIndex(f => f.Id == filterchain.Id);
        }

       /// <summary>
        /// adds the given Filterchain to the Filtergraph
        /// </summary>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Add(Filterchain filterchain)
        {
            filterchain.Owner = this;
            FilterchainList.Add(filterchain);
            return this;
        }

        /// <summary>
        /// merges the given Filterchain to the Filtergraph
        /// </summary>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        /// <param name="optionType">the option specifying how the merge should declare a winner</param>
        public Filtergraph Merge(Filterchain filterchain, FFmpegMergeOptionType optionType)
        {
            var indexOfItem = IndexOf(filterchain); 
            if (indexOfItem != -1 && optionType == FFmpegMergeOptionType.NewWins)
            {
                FilterchainList.RemoveAt(indexOfItem);
                filterchain.Owner = this;
                FilterchainList.Insert(indexOfItem, filterchain);
            }
            else if (indexOfItem == -1)
            {
                Add(filterchain);       
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
        public Filtergraph RemoveAll(Predicate<Filterchain> pred)
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
        internal FFmpegCommand Owner { get; set; }
        internal List<Filterchain> FilterchainList { get; set; }
        #endregion
    }
}
