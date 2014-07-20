using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// a collecton of <see cref="TCollection"/>, that will be validated to only include objects which contain an AppliesToResource attribute that is a type of this.
    /// </summary>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    public class ForStreamCollection<TCollection>
    {
        private readonly Type _restrictedType;

        public ForStreamCollection(Type restrictedType)
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
            return List.Any(f => f is TItem);
        }

        public bool Contains<TItem>(TItem item)
            where TItem : TCollection
        {
            var itemType = item.GetType();
            return List.Any(f => f.GetType().IsAssignableFrom(itemType));
        }

        public int IndexOf<TItem>()
            where TItem : TCollection
        {
            return List.FindIndex(f => f is TItem);
        }

        public int IndexOf<TItem>(TItem item)
            where TItem : TCollection
        {
            var itemType = item.GetType();
            return List.FindIndex(f => f.GetType().IsAssignableFrom(itemType));
        }

        public ForStreamCollection<TCollection> Merge<TItem>(TItem item, FfmpegMergeOptionType optionType)
            where TItem : TCollection
        {
            var applierType = item.GetType();
            if (!Validate.AppliesTo(applierType, _restrictedType))
            {
                throw new ForStreamInvalidException(applierType, _restrictedType); 
            }

            var indexOfItem = IndexOf(item); 
            if (indexOfItem != -1 && optionType == FfmpegMergeOptionType.NewWins)
            {
                List.RemoveAt(indexOfItem);
                List.Insert(indexOfItem, item);
            }
            else if (indexOfItem == -1)
            {
                List.Add(item);       
            }

            return this;
        }

        public ForStreamCollection<TCollection> Add<TItem>(TItem item)
            where TItem : TCollection
        {
            var applierType = item.GetType();
            if (!Validate.AppliesTo(applierType, _restrictedType))
            {
                throw new ForStreamInvalidException(applierType, _restrictedType); 
            }

            if (Contains(item))
            {
                throw new InvalidOperationException(string.Format("A member '{0}' already exists in the collection.", applierType.Name));
            }

            List.Add(item);

            return this;
        }

        public ForStreamCollection<TCollection> AddRange(params TCollection[] list)
        {
            foreach (var item in list)
            {
                Add(item);
            }

            return this;
        }

        public ForStreamCollection<TCollection> Remove<TItem>()
            where TItem : TCollection
        {
            if (Contains<TItem>())
            {
                List.RemoveAll(f => f is TItem);
            }
            return this;
        }

        public TItem Get<TItem>()
            where TItem : class, TCollection
        {
            return List.First(f => f is TItem) as TItem;
        }

        public ForStreamCollection<TCollection> RemoveAt(int index)
        {
            List.RemoveAt(index);

            return this;
        }

        public ForStreamCollection<TCollection> RemoveAll(Predicate<TCollection> pred)
        {
            List.RemoveAll(pred);

            return this;
        }

        public TCollection LastOrDefault()
        {
            return List.LastOrDefault(); 
        }

        public TCollection FirstOrDefault()
        {
            return List.FirstOrDefault();
        }

        #region Internals
        internal List<TCollection> List { get; set; }
        #endregion
    }
}
