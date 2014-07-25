using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public abstract class BaseSplit : 
        BaseFilter,
        IFilterProcessor,
        IFilterValidator, 
        IFilterMultiOutput
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "split";

        protected BaseSplit(string filterPrefix)
            : base(string.Concat(filterPrefix, FilterType), FilterMaxInputs)
        {
        }

        public int? NumberOfStreams { get; set; }

        public override void Validate()
        {
            if (NumberOfStreams.HasValue && NumberOfStreams < 2)
            {
                throw new InvalidOperationException("Number Of Streams must be greater or equal to 2 for a split filter.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (NumberOfStreams.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, NumberOfStreams.GetValueOrDefault());
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }

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
