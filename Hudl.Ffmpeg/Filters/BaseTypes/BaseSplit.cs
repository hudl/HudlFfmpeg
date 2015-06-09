using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public abstract class BaseSplit : 
        IFilter,
        IFilterValidator, 
        IFilterProcessor,
        IFilterMultiOutput
    {
        [FilterParameter]
        [Validate(LogicalOperators.GreaterThanOrEqual, 2)]
        public int? NumberOfStreams { get; set; }

        #region IFilterMultiOutput
        public int OutputCount(FilterMultiOutputContext context)
        {
            return NumberOfStreams ?? 2;
        }
        #endregion

        #region IFilterValidator 
        public bool Validate(FilterValidatorContext context)
        {
            return context.NumberOfFiltersInFilterchain == 1; 
        }
        #endregion

        #region IFilterProcessor
        public void Process(FilterProcessorContext context)
        {
            var firstStream = context.Streams.OfType<VideoStream>().FirstOrDefault();
            if (firstStream == null)
            {
                throw new InvalidOperationException("Found a spit filter with zero video streams.");
            }

            for (var i = context.Filterchain.OutputCount; i < NumberOfStreams; i++)
            {
                context.Filterchain.CreateOutput(firstStream.Copy());
            } 
        }
        #endregion



        
    }

}
