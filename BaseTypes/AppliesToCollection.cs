using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.BaseTypes
{
    /// <summary>
    /// a collecton of <see cref="TypeA"/>, that will be validated to only include those that are applicable to <see cref="TypeB"/>
    /// </summary>
    /// <typeparam name="TypeA">the type of the collection</typeparam>
    /// <typeparam name="TypeB">the type the collection must apply to</typeparam>
    internal class AppliesToCollecion<TypeA, TypeB>
        where TypeB : IResource
    {
        private new List<TypeA> _filterList;
        public readonly IReadOnlyList<TypeA> Items
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

        public AppliesToCollecion<TypeA, TypeB> Add(TypeA item)
        {
            if (Validate.AppliesTo<TypeA, TypeB>())
            {
                throw new AppliesToInvalidException<TypeA, TypeB>();
            }

            _filterList.Add(item);

            return this;
        }

        public AppliesToCollecion<TypeA, TypeB> AddRange(params TypeA[] list)
        {
            foreach (TypeA item in list)
            {
                Add(item);
            }

            return this;
        }

        public AppliesToCollecion<TypeA, TypeB> Remove(int index)
        {
            _filterList.RemoveAt(index);

            return this;
        }

        public AppliesToCollecion<TypeA, TypeB> RemoveAll(Predicate<TypeA> pred)
        {
            _filterList.RemoveAll(pred);

            return this;
        }
    }
}
