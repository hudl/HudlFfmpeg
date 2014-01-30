using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class Filterchainv2
    {
        private Filterchainv2(List<IResource> outputsToUse) 
        {
            if (outputsToUse == null || outputsToUse.Count == 0)
            {
                throw new ArgumentNullException("outputsToUse");
            }

            Id = Guid.NewGuid().ToString();
            ReceiptList = new List<CommandReceipt>();
            OutputList = new List<FilterchainOutputv2>(); 
            Filters = new AppliesToCollection<IFilter>(outputsToUse.First().GetType());
            OutputList.AddRange(outputsToUse.Select(output => new FilterchainOutputv2(this, output)));
        }
        private Filterchainv2(List<IResource> outputsToUse, params IFilter[] filters)
            : this(outputsToUse)
        {
            if (filters.Length > 0)
            {
                Filters.AddRange(filters); 
            }
        }

        public static Filterchainv2 Create(List<IResource> outputsToUse, params IFilter[] filters)
        {
            return new Filterchainv2(outputsToUse, filters);
        }

        public AppliesToCollection<IFilter> Filters { get; protected set; }

        public ReadOnlyCollection<CommandReceipt> Resources { get { return ReceiptList.AsReadOnly(); } }

        public void SetResources(params CommandReceipt[] receipts)
        {
            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }
            if (receipts.Length == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            SetResources(new List<CommandReceipt>(receipts));
        }

        public void SetResources(List<CommandReceipt> receipts)
        {
            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }
            if (receipts.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            ReceiptList = receipts;
        }

        public Filterchainv2 Copy()
        {
            var clonedResources = OutputList.Select(output => output.Resource.Copy<IResource>());
            return Filterchain.FilterTo(clonedResources.ToList(), Filters.List.ToArray());
        }

        public List<FilterchainOutputv2> Outputs(Commandv2 command)
        {
            return OutputList.Select(output =>
                {
                    output.Length = TimeSpan.FromSeconds(Helpers.GetLength(command, this));
                    return output;
                }).ToList();
        }

        public List<CommandReceipt> GetReceipts()
        {
            return OutputList.Select(output => 
                    CommandReceipt.CreateFromStream(Owner.Owner.Owner.Id, Owner.Owner.Id, output.Resource.Map))
                    .ToList();
        }

        #region Internals
        internal string Id { get; set; }
        internal Filtergraphv2 Owner { get; set; }
        internal List<CommandReceipt> ReceiptList { get; set; }
        internal List<FilterchainOutputv2> OutputList { get; set; }
        #endregion
    }
}
