using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// A series of Filterchains that work together to convert Video, Audio, and Image streams.
    /// </summary>
    public class Filtergraph
    {
        private new List<Filterchain<IResource>> _filterchains;
        public readonly IReadOnlyList<Filterchain<IResource>> Filterchains { get { return _filterchains.AsReadOnly(); } }

        public TypeA Assign<TypeA>(Filterchain<TypeA> filterchain, params IResource[] resources)
            where TypeA : IResource
        {
            return filterchain.Output;
        }

        public TypeA AssignToAll<TypeA>(Filterchain<TypeA> filterchain)
            where TypeA : IResource
        {
            return filterchain.Output; 
        }

        /// <summary>
        /// adds the given Filterchain to the Filtergraph
        /// </summary>
        /// <typeparam name="TypeA">the generic type of the filterchain</typeparam>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Add<TypeA>(TypeA filterchain)
            where TypeA : Filterchain<IResource>
        {
            _filterchains.Add(filterchain);
            return this;
        }
       
        /// <summary>
        /// removes the Filterchain at the given index from the Filtergraph
        /// </summary>
        /// <param name="index">the index of the desired Filterchain to be removed from the Filtergraph</param>
        public Filtergraph Remove(int index)
        {
            _filterchains.RemoveAt(index);
            return this;
        }
         
        /// <summary>
        /// removes all the Filterchain matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public Filtergraph RemoveAll(Predicate<Filterchain<IResource>> pred)
        {
            _filterchains.RemoveAll(pred);
            return this; 
        }

        public override string ToString() 
        {
            //perform simple validation on filter graph
            if (_filterchains.Count == 0)
                throw new ArgumentException("Filtergraph must contain at least one Filterchain.");

            StringBuilder filtergraph = new StringBuilder(100);
            _filterchains.ForEach(filterchain =>
                {
                    if (filtergraph.Length > 0) filtergraph.Append(";");
                    filtergraph.Append(filterchain.ToString());
                });

            //return the formatted filter command string 
            return filtergraph.ToString();
        }
    }
}
