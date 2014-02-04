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
    public class Filterchain
    {
        private Filterchain(List<IResource> outputsToUse) 
        {
            if (outputsToUse == null || outputsToUse.Count == 0)
            {
                throw new ArgumentNullException("outputsToUse");
            }

            Id = Guid.NewGuid().ToString();
            ReceiptList = new List<CommandReceipt>();
            OutputList = new List<FilterchainOutput>(); 
            Filters = new AppliesToCollection<IFilter>(outputsToUse.First().GetType());
            OutputList.AddRange(outputsToUse.Select(output => new FilterchainOutput(this, output)));
        }
        private Filterchain(List<IResource> outputsToUse, params IFilter[] filters)
            : this(outputsToUse)
        {
            if (filters.Length > 0)
            {
                Filters.AddRange(filters); 
            }
        }

        public static Filterchain Create(List<IResource> outputsToUse, params IFilter[] filters)
        {
            return new Filterchain(outputsToUse, filters);
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

        public Filterchain Copy()
        {
            var clonedResources = OutputList.Select(output => output.Resource.Copy<IResource>());
            return FilterTo(clonedResources.ToList(), Filters.List.ToArray());
        }

        public List<FilterchainOutput> Outputs(FfmpegCommand command)
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

        public static Filterchain FilterTo<TResource>(params IFilter[] filters)
            where TResource : IResource, new()
        {
            return FilterTo<TResource>(1, filters);
        }

        public static Filterchain FilterTo<TResource>(int count, params IFilter[] filters)
            where TResource : IResource, new()
        {
            var outputList = new List<IResource>();
            for (var i = 0; i < count; i++)
            {
                outputList.Add(new TResource());
            }
            return FilterTo(outputList, filters);
        }

        public static Filterchain FilterTo(List<IResource> outputsToUse, params IFilter[] filters)
        {
            if (outputsToUse == null || outputsToUse.Count == 0)
            {
                throw new ArgumentException("Outputs specified cannot be null or empty.", "outputsToUse");
            }

            return Create(outputsToUse, filters);
        }

        #region Internals
        internal string Id { get; set; }
        internal Filtergraph Owner { get; set; }
        internal List<CommandReceipt> ReceiptList { get; set; }
        internal List<FilterchainOutput> OutputList { get; set; }
        #endregion
    }
}
