using System;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class FilterchainOutputv2
    {
        internal FilterchainOutputv2(Filterchainv2 owner, IResource resource)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            Owner = owner;
            Resource = resource;
            Id = Guid.NewGuid().ToString();
        }

        public TimeSpan Length { get; set; }

        public Filterchainv2 Owner { get; protected set; }

        public IResource Output()
        {
            Resource.Length = Length;
            return Resource;
        }

        public FilterchainOutputv2 Copy()
        {
            return new FilterchainOutputv2(Owner, Output().Copy<IResource>());
        }

        #region Internals
        internal string Id { get; set; }
        internal IResource Resource { get; set; }
        #endregion 
    }
}
