using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    internal class Utilities
    {
        public static int GetFilterOutputMax(Filterchainv2 filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
            
            return filterchain.Filters.List.Max(f =>
                {
                    if (!(f is IFilterMultiOutput))
                    {
                        return 1;
                    }
                    return (f as IFilterMultiOutput).OutputCount();
                });
        }

        public static int GetFilterInputMax(Filterchainv2 filterchain)
        {
            return filterchain.Filters.List.Min(f => f.MaxInputs);
        }

        public static bool ValidateFilters(Commandv2 command, Filterchainv2 filterchain, List<CommandReceipt> receipts)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            return filterchain.Filters.List.TrueForAll(f =>
            {
                if (!(f is IFilterValidator))
                {
                    return true;
                }
                return (f as IFilterValidator).Validate(command, filterchain, receipts);
            });
        }

        public static bool ValidateFiltersMax(Filterchainv2 filterchain, List<CommandReceipt> resources)
        {
            var maximumAllowedMinimum = GetFilterInputMax(filterchain);
            return maximumAllowedMinimum > 1 || (maximumAllowedMinimum == 1 && resources.Count == 1);
        }

        public static void ProcessFilters(Commandv2 command, Filterchainv2 filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            filterchain.Filters.List.ForEach(filter =>
            {
                if (!(filter is IFilterProcessor))
                {
                    return;
                }

                (filter as IFilterProcessor).PrepCommands(command, filterchain);
            });
        }
    }
}
