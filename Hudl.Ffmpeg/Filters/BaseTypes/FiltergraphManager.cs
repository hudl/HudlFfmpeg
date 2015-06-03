using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    /// <summary>
    /// A manager that controls the list of filterchains for a filtergraph.
    /// </summary>
    public class FiltergraphManager
    {
        private FiltergraphManager(FFmpegCommand owner)
        {
            Owner = owner;
        }

        private FFmpegCommand Owner { get; set; }

        public List<StreamIdentifier> Add(Filterchain filterchain, params StreamIdentifier[] streamIds)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
            if (streamIds == null || streamIds.Length == 0)
            {
                throw new ArgumentException("Cannot apply filters to null or empty objects.", "streamIds");
            }

            var streamIdList = new List<StreamIdentifier>(streamIds);
            if (!streamIdList.TrueForAll(streamId => Owner.Objects.ContainsInput(streamId) || Owner.Objects.ContainsStream(streamId)))
            {
                throw new ArgumentException("Cannot apply filters to inputs or streams that do not exist in the command.", "streamIds");
            }

            var finalFilter = filterchain.Filters.LastOrDefault();
            if (finalFilter == null)
            {
                throw new ArgumentException("Filterchain must contain at least one filter.", "filterchain");
            }

            if (!Utilities.ValidateFiltersMax(filterchain, streamIdList))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, exceeds maximum calculated allowable resources.");
            }

            if (!Utilities.ValidateFilters(Owner, filterchain, streamIdList))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, failed to comply with child filter requirements.");
            }

            var maximumInputs = Utilities.GetFilterInputMax(filterchain);

            Filterchain finalFilterchain = null;
            var segmentsList = Helpers.BreakStreamIdentifiers(maximumInputs, streamIds);
            segmentsList.ForEach(segment =>
                {
                    var segmentList = new List<StreamIdentifier>(segment);
                    if (finalFilterchain != null)
                    {
                        finalFilterchain.GetStreamIdentifiers().ForEach(r => segmentList.Insert(0, r));
                    }

                    finalFilterchain = filterchain.Copy();

                    finalFilterchain.Owner = Owner.Objects.Filtergraph; 

                    finalFilterchain.SetResources(segmentList);

                    Utilities.ProcessFilters(Owner, finalFilterchain);

                    Owner.Objects.Filtergraph.Add(finalFilterchain);
                });

            if (finalFilterchain == null)
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, segemented filters caused an unrecoverable issue.");
            }

            return finalFilterchain.GetStreamIdentifiers(); 
        }

        public List<StreamIdentifier> AddToEach(Filterchain filterchain, params StreamIdentifier[] streamIds)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
            if (streamIds == null || streamIds.Length == 0)
            {
                throw new ArgumentException("Cannot apply filters to null or empty objects.", "streamIds");
            }

            var resourceList = new List<StreamIdentifier>(streamIds);

            return resourceList.SelectMany(r => Add(filterchain, r)).ToList();
        }

        internal static FiltergraphManager Create(FFmpegCommand owner)
        {
            return new FiltergraphManager(owner);
        }
    }
}
