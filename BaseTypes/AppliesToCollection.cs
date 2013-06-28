using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// a collecton of <see cref="TCollection"/>, that will be validated to only include those that are applicable to <see cref="TRestrictedTo"/>
    /// </summary>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    /// <typeparam name="TRestrictedTo">the type the collection must apply to</typeparam>
    public class AppliesToCollecion<TCollection, TRestrictedTo>
        where TRestrictedTo : IResource
    {
        private readonly List<TCollection> _filterList = new List<TCollection>();

        internal List<TCollection> List { get { return _filterList; } } 

        public IReadOnlyList<TCollection> Items
        { 
            get 
            { 
                return _filterList.AsReadOnly(); 
            } 
        }

        public int Count 
        { 
            get 
            { 
                return Items.Count; 
            } 
        }

        public AppliesToCollecion<TCollection, TRestrictedTo> Add(TCollection item)
        {
            if (Validate.AppliesTo<TCollection, TRestrictedTo>())
            {
                throw new AppliesToInvalidException<TCollection, TRestrictedTo>();
            }

            _filterList.Add(item);

            return this;
        }

        public AppliesToCollecion<TCollection, TRestrictedTo> AddRange(params TCollection[] list)
        {
            foreach (var item in list)
            {
                Add(item);
            }

            return this;
        }

        public AppliesToCollecion<TCollection, TRestrictedTo> Remove(int index)
        {
            _filterList.RemoveAt(index);

            return this;
        }

        public AppliesToCollecion<TCollection, TRestrictedTo> RemoveAll(Predicate<TCollection> pred)
        {
            _filterList.RemoveAll(pred);

            return this;
        }
    }
}
