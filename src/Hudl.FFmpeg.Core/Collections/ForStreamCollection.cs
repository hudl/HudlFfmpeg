using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Exceptions;

namespace Hudl.FFmpeg.Collections
{
    /// <summary>
    /// a collecton of <see cref="TCollection"/>, that will be validated to only include objects which contain an AppliesToResource attribute that is a type of this.
    /// </summary>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    public class ForStreamCollection<TCollection> : IEnumerable<TCollection>
    {
        private readonly Type _restrictedType;
        private readonly List<TCollection> _collection;

        public ForStreamCollection(Type restrictedType)
        {
            if (restrictedType == null)
            {
                throw new ArgumentNullException("restrictedType");
            }

            _collection = new List<TCollection>();
            _restrictedType = restrictedType;
        }

        public int Count { get { return _collection.Count; } }

        public bool Contains<TItem>()
            where TItem : TCollection
        {
            return _collection.OfType<TItem>().Any();
        }

        public bool Contains<TItem>(TItem item)
            where TItem : TCollection
        {
            var itemType = item.GetType();
            return _collection.Any(f => f.GetType().IsAssignableFrom(itemType));
        }

        public int IndexOf<TItem>()
            where TItem : TCollection
        {
            return _collection.FindIndex(f => f is TItem);
        }

        public int IndexOf<TItem>(TItem item)
            where TItem : TCollection
        {
            var itemType = item.GetType();
            return _collection.FindIndex(f => f.GetType().IsAssignableFrom(itemType));
        }

        public ForStreamCollection<TCollection> Merge<TItem>(TItem item, FFmpegMergeOptionType optionType)
            where TItem : TCollection
        {
            var applierType = item.GetType();
            if (!AttributeValidation.AttributeTypeEquals(applierType, _restrictedType))
            {
                throw new ForStreamInvalidException(applierType, _restrictedType); 
            }

            var indexOfItem = IndexOf(item); 
            if (indexOfItem != -1 && optionType == FFmpegMergeOptionType.NewWins)
            {
                _collection.RemoveAt(indexOfItem);
                _collection.Insert(indexOfItem, item);
            }
            else if (indexOfItem == -1)
            {
                _collection.Add(item);       
            }

            return this;
        }

        public ForStreamCollection<TCollection> Add<TItem>(TItem item)
            where TItem : TCollection
        {
            var applierType = item.GetType();
            if (!AttributeValidation.AttributeTypeEquals(applierType, _restrictedType))
            {
                throw new ForStreamInvalidException(applierType, _restrictedType); 
            }

            if (Contains(item))
            {
                throw new InvalidOperationException(string.Format("A member '{0}' already exists in the collection.", applierType.Name));
            }

            _collection.Add(item);

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
                _collection.RemoveAll(f => f is TItem);
            }
            return this;
        }

        public TItem Get<TItem>()
            where TItem : class, TCollection
        {
            return _collection.First(f => f is TItem) as TItem;
        }

        public ForStreamCollection<TCollection> RemoveAt(int index)
        {
            _collection.RemoveAt(index);

            return this;
        }

        public ForStreamCollection<TCollection> RemoveAll(Predicate<TCollection> pred)
        {
            _collection.RemoveAll(pred);

            return this;
        }

        public IEnumerator<TCollection> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_collection).GetEnumerator();
        }
    }
}
