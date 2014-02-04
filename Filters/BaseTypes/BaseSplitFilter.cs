using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public abstract class BaseSplitFilter : 
        BaseFilter,
        IFilterProcessor,
        IFilterValidator, 
        IFilterMultiOutput
    {

        protected BaseSplitFilter(string type, int maxInputs)
            : base(type, maxInputs)
        {
        }
        protected BaseSplitFilter(string type, int maxInputs, int numberOfStreams)
            : this(type, maxInputs)
        {
            NumberOfStreams = numberOfStreams;
        }

        public int NumberOfStreams { get; set; }
        
        public override string ToString()
        {
            if (NumberOfStreams <= 1)
            {
                throw new InvalidOperationException("NumberOfStreams must be greater than one for a split.");
            }

            return string.Concat(Type, "=", NumberOfStreams.ToString(CultureInfo.InvariantCulture));
        }

        #region IFilterMultiOutput
        public int OutputCount()
        {
            return NumberOfStreams;
        }
        #endregion

        #region IFilterProcessor
        public bool Validate(Commandv2 command, Filterchainv2 filterchain, List<CommandReceipt> receipts)
        {
            return filterchain.Filters.Count == 1;
        }
        #endregion

        #region IFilterValidator
        public void PrepCommands(Commandv2 command, Filterchainv2 filterchain)
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
