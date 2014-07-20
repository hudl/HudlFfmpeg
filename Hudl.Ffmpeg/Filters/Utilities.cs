using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    internal class Utilities
    {
        public static int GetFilterOutputMax(Filterchain filterchain)
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

        public static int GetFilterInputMax(Filterchain filterchain)
        {
            return filterchain.Filters.List.Min(f => f.MaxInputs);
        }

        public static bool ValidateFilters(FfmpegCommand command, Filterchain filterchain, List<StreamIdentifier> streamIds)
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
                return (f as IFilterValidator).Validate(command, filterchain, streamIds);
            });
        }

        public static bool ValidateFiltersMax(Filterchain filterchain, List<StreamIdentifier> resources)
        {
            var maximumAllowedMinimum = GetFilterInputMax(filterchain);
            return maximumAllowedMinimum > 1 || (maximumAllowedMinimum == 1 && resources.Count == 1);
        }

        public static void ProcessFilters(FfmpegCommand command, Filterchain filterchain)
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
