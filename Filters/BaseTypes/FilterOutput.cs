using System;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class FilterOutput<TResource>
        where TResource : IResource
    {
        internal FilterOutput(Filterchain<IResource> parent, TResource resource)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            Parent = parent;
            Resource = resource;
        }

        public Filterchain<IResource> Parent { get; protected set; }

        //public TimeSpan Length
        //{
        //    get
        //    {
        //        return TimeSpan.FromSeconds(Helpers.GetLength(Parent as Filterchain<IResource>));
        //    }
        //}

        //public TResource Output()
        //{
        //    Resource.Length = Length;
        //    return Resource;
        //}

        #region Internals
        internal TResource Resource { get; set; }
        #endregion 
    }
}
