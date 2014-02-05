using System;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class FilterchainOutput
    {
        internal FilterchainOutput(Filterchain owner, IResource resource)
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

        public Filterchain Owner { get; protected set; }

        public IResource Output()
        {
            Resource.Length = Length;
            return Resource;
        }

        public FilterchainOutput Copy()
        {
            return new FilterchainOutput(Owner, Output().Copy<IResource>());
        }

        public CommandReceipt GetReceipt()
        {
            return CommandReceipt.CreateFromOutput(Owner.Owner.Owner.Id, Owner.Id, Resource.Map);
        }

        #region Internals
        internal string Id { get; set; }
        internal IResource Resource { get; set; }
        #endregion 
    }
}
