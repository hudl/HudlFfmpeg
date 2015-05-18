using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public abstract class BaseSplit : 
        IFilter,
        IFilterProcessor,
        IFilterValidator, 
        IFilterMultiOutput
    {
        [FilterParameter]
        [FilterParameterValidator(LogicalOperators.GreaterThanOrEqual, 2)]
        public int? NumberOfStreams { get; set; }

        #region IFilterMultiOutput
        public int OutputCount()
        {
            return NumberOfStreams ?? 2;
        }
        #endregion

        #region IFilterProcessor
        public bool Validate(FFmpegCommand command, Filterchain filterchain, List<StreamIdentifier> streamIds)
        {
            return filterchain.Filters.Count == 1;
        }
        #endregion

        #region IFilterValidator
        public void PrepCommands(FFmpegCommand command, Filterchain filterchain)
        {
            if (filterchain.OutputList.Count == 0)
            {
                throw new InvalidOperationException("A split cannot happen without a demo output to split on.");
            }

            if (filterchain.OutputList.Count > NumberOfStreams)
            {
                throw new InvalidOperationException("A split cannot happen when the supplied filterchain has to many demo outputs.");
            }

            if (filterchain.OutputList.Count == NumberOfStreams)
            {
                return;
            }

            for (var i = filterchain.OutputList.Count; i < NumberOfStreams; i++)
            {
                filterchain.OutputList.Add(filterchain.OutputList
                                                      .First()
                                                      .Copy());
            }
        }
        #endregion
    }

}
