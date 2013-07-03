using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// a collecton of <see cref="TCollection"/>, that will be validated to only include those that are applicable to <see cref="TRestrictedTo"/>
    /// </summary>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    /// <typeparam name="TRestrictedTo">the type the collection must apply to</typeparam>
    public class AppliesToCollection<TCollection, TRestrictedTo>
        where TRestrictedTo : IResource
    {
        public AppliesToCollection()
        {
            List = new List<TCollection>();
        }

        public IReadOnlyList<TCollection> Items
        { 
            get 
            { 
                return List.AsReadOnly(); 
            } 
        }

        public int Count 
        { 
            get 
            {
                return List.Count; 
            } 
        }

        public AppliesToCollection<TCollection, TRestrictedTo> Add<TItem>(TItem item)
            where TItem : TCollection
        {
            var applierType = item.GetType();
            var appliedTo = typeof (TRestrictedTo);
            if (!Validate.AppliesTo(applierType, appliedTo))
            {
                throw new AppliesToInvalidException(applierType, appliedTo); 
            }
            if (Contains(item))
            {
                throw new InvalidOperationException(string.Format("A member '{0}' already exists in the collection.", applierType.Name));
            }

            List.Add(item);

            return this;
        }

        public AppliesToCollection<TCollection, TRestrictedTo> AddRange(params TCollection[] list)
        {
            foreach (var item in list)
            {
                Add(item);
            }

            return this;
        }

        public AppliesToCollection<TCollection, TRestrictedTo> Remove<TItem>()
        {
            if (Contains<TItem>())
            {
                List.RemoveAll(f => f is TItem);
            }
            return this;
        }

        public bool Contains<TItem>()
        {
            return (List.Count(f => f is TItem) > 0);
        }

        public bool Contains<TItem>(TItem item)
            where TItem : TCollection
        {
            var itemType = item.GetType();
            return (List.Count(f => f.GetType().IsAssignableFrom(itemType)) > 0);
        }

        public AppliesToCollection<TCollection, TRestrictedTo> RemoveAt(int index)
        {
            List.RemoveAt(index);

            return this;
        }

        public AppliesToCollection<TCollection, TRestrictedTo> RemoveAll(Predicate<TCollection> pred)
        {
            List.RemoveAll(pred);

            return this;
        }

        #region Internals
        internal List<TCollection> List { get; set; }
        #endregion
    }
}
