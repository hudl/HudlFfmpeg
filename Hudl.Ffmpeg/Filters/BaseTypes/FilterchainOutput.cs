using System;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public class FilterchainOutput
    {
        private FilterchainOutput(Filterchain owner, IStream resource)
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
            Stream = resource;
            Id = Guid.NewGuid().ToString();
        }

        public Filterchain Owner { get; protected set; }
        
        public IStream Stream { get; protected set; }

        public FilterchainOutput Copy()
        {
            return new FilterchainOutput(Owner, Stream.Copy());
        }

        public StreamIdentifier GetStreamIdentifier()
        {
            return StreamIdentifier.Create(Owner.Owner.Owner.Id, Owner.Id, Stream.Map);
        }

        internal static FilterchainOutput Create(Filterchain owner, IStream resource)
        {
            return new FilterchainOutput(owner, resource);
        }

        #region Internals
        internal string Id { get; set; }
        #endregion 
    }
}
