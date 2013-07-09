using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class FilterchainOutput<TResource>
        where TResource : IResource
    {
        internal FilterchainOutput(Filterchain<TResource> parent, TResource resource)
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

        public TimeSpan Length { get; set; }

        public Filterchain<TResource> Parent { get; protected set; }

        public TResource GetOutput()
        {
            Resource.Length = Length;
            return Resource;
        }

        #region Internals
        internal TResource Resource { get; set; }
        #endregion 
    }
}
