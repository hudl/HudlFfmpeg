using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters
{
    internal class Utilities
    {
        public static int GetFilterOutputMax(Filterchain filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var context = new FilterMultiOutputContext
                {
                    NumberOfOutputsInFilterchain = filterchain.OutputList.Count
                };

            return filterchain.Filters.Max(f =>
                {
                    if (!(f is IFilterMultiOutput))
                    {
                        return 1;
                    }
                    return (f as IFilterMultiOutput).OutputCount(context);
                });
        }

        public static int GetFilterInputMax(Filterchain filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            return filterchain.Filters.Min(f =>
                {
                    var filterAttribute = AttributeRetrieval.GetAttribute<FilterAttribute>(f.GetType());

                    return filterAttribute.MaxInputs;
                });
        }

        public static bool ValidateFilters(FFmpegCommand command, Filterchain filterchain, List<StreamIdentifier> streamIds)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var context = new FilterValidatorContext
                {
                    NumberOfFiltersInFilterchain = filterchain.Filters.Count
                };

            return filterchain.Filters.ToList().TrueForAll(f =>
            {
                if (!(f is IFilterValidator))
                {
                    return true;
                }
                return (f as IFilterValidator).Validate(context);
            });
        }

        public static bool ValidateFiltersMax(Filterchain filterchain, List<StreamIdentifier> resources)
        {
            var maximumAllowedMinimum = GetFilterInputMax(filterchain);

            return maximumAllowedMinimum > 1 || (maximumAllowedMinimum == 1 && resources.Count == 1);
        }

        public static void ProcessFilters(FFmpegCommand command, Filterchain filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var context = new FilterProcessorContext
                {
                    Filterchain = filterchain, 
                    Streams = filterchain.OutputList.Select(o => o.Stream).ToList()
                };

            filterchain.Filters.ToList().ForEach(filter =>
            {
                if (!(filter is IFilterProcessor))
                {
                    return;
                }

                (filter as IFilterProcessor).Process(context);
            });
        }
    }
}
