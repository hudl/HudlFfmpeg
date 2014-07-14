using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;

namespace Hudl.Ffmpeg.Command.Managers
{
    /// <summary>
    /// A manager that controls the list of filterchains for a filtergraph.
    /// </summary>
    public class CommandFiltergraphManager
    {
        private CommandFiltergraphManager(FfmpegCommand owner)
        {
            Owner = owner;
        }

        private FfmpegCommand Owner { get; set; }

        public List<CommandReceipt> Add(Filterchain filterchain, params CommandReceipt[] receipts)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
            if (receipts == null || receipts.Length == 0)
            {
                throw new ArgumentException("Cannot apply filters to null or empty objects.", "receipts");
            }

            var receiptList = new List<CommandReceipt>(receipts);
            if (!receiptList.TrueForAll(receipt => Owner.Objects.ContainsInput(receipt) || Owner.Objects.ContainsStream(receipt)))
            {
                throw new ArgumentException("Cannot apply filters to inputs or streams that do not exist in the command.", "receipts");
            }

            var finalFilter = filterchain.Filters.LastOrDefault();
            if (finalFilter == null)
            {
                throw new ArgumentException("Filterchain must contain at least one filter.", "filterchain");
            }

            if (!Filters.Utilities.ValidateFiltersMax(filterchain, receiptList))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, exceeds maximum calculated allowable resources.");
            }

            if (!Filters.Utilities.ValidateFilters(Owner, filterchain, receiptList))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, failed to comply with child filter requirements.");
            }

            var maximumInputs = Filters.Utilities.GetFilterInputMax(filterchain);

            Filterchain finalFilterchain = null;
            var segmentsList = Helpers.BreakReceipts(maximumInputs, receipts);
            segmentsList.ForEach(segment =>
                {
                    var segmentList = new List<CommandReceipt>(segment);
                    if (finalFilterchain != null)
                    {
                        finalFilterchain.GetReceipts().ForEach(r => segmentList.Insert(0, r));
                    }

                    finalFilterchain = filterchain.Copy();

                    finalFilterchain.Owner = Owner.Objects.Filtergraph; 

                    finalFilterchain.SetResources(segmentList);

                    Filters.Utilities.ProcessFilters(Owner, finalFilterchain);

                    Owner.Objects.Filtergraph.Add(finalFilterchain);
                });

            if (finalFilterchain == null)
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, segemented filters caused an unrecoverable issue.");
            }

            return finalFilterchain.GetReceipts(); 
        }

        public List<CommandReceipt> AddToEach(Filterchain filterchain, params CommandReceipt[] receipts)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
            if (receipts == null || receipts.Length == 0)
            {
                throw new ArgumentException("Cannot apply filters to null or empty objects.", "receipts");
            }

            var resourceList = new List<CommandReceipt>(receipts);

            return resourceList.SelectMany(r => Add(filterchain, r)).ToList();
        }

        internal static CommandFiltergraphManager Create(FfmpegCommand owner)
        {
            return new CommandFiltergraphManager(owner);
        }
    }
}
