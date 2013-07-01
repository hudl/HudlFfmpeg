using System;
using System.Collections.Generic;
using System.Text;
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

        public IReadOnlyList<Filterchain<IResource>> Filterchains
        {
            get
            {
                return FilterchainList.AsReadOnly();
            }
        }

        /// <summary>
        /// adds the given Filterchain to the Filtergraph
        /// </summary>
        /// <typeparam name="TOutput">the generic type of the filterchain</typeparam>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Add<TOutput>(TOutput filterchain)
            where TOutput : Filterchain<IResource>
        {
            FilterchainList.Add(filterchain);
            return this;
        }
       
        /// <summary>
        /// removes the Filterchain at the given index from the Filtergraph
        /// </summary>
        /// <param name="index">the index of the desired Filterchain to be removed from the Filtergraph</param>
        public Filtergraph Remove(int index)
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
                throw new ArgumentException("Filtergraph must contain at least one Filterchain.");
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
