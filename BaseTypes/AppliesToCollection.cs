using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// a collecton of <see cref="TCollection"/>, that will be validated to only include those that are applicable to the RestrictionType
    /// </summary>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    public class AppliesToCollection<TCollection>
    {
        private readonly Type _restrictedType;

        public AppliesToCollection(Type restrictedType)
        {
            if (restrictedType == null)
            {
                throw new ArgumentNullException("restrictedType");
            }

            List = new List<TCollection>();
            _restrictedType = restrictedType;
        }

        public ReadOnlyCollection<TCollection> Items
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

        public bool Contains<TItem>()
            where TItem : TCollection
        {
            return (List.Count(f => f is TItem) > 0);
        }

        public bool Contains<TItem>(TItem item)
            where TItem : TCollection
        {
            var itemType = item.GetType();
            return (List.Count(f => f.GetType().IsAssignableFrom(itemType)) > 0);
        }

        public AppliesToCollection<TCollection> Add<TItem>(TItem item)
            where TItem : TCollection
        {
            var applierType = item.GetType();
            if (!Validate.AppliesTo(applierType, _restrictedType))
            {
                throw new AppliesToInvalidException(applierType, _restrictedType); 
            }
            if (Contains(item))
            {
                throw new InvalidOperationException(string.Format("A member '{0}' already exists in the collection.", applierType.Name));
            }

            List.Add(item);

            return this;
        }

        public AppliesToCollection<TCollection> AddRange(params TCollection[] list)
        {
            foreach (var item in list)
            {
                Add(item);
            }

            return this;
        }

        public AppliesToCollection<TCollection> Remove<TItem>()
            where TItem : TCollection
        {
            if (Contains<TItem>())
            {
                List.RemoveAll(f => f is TItem);
            }
            return this;
        }

        public AppliesToCollection<TCollection> RemoveAt(int index)
        {
            List.RemoveAt(index);

            return this;
        }

        public AppliesToCollection<TCollection> RemoveAll(Predicate<TCollection> pred)
        {
            List.RemoveAll(pred);

            return this;
        }

        #region Internals
        internal List<TCollection> List { get; set; }
        #endregion
    }
}
