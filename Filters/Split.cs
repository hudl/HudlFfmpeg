using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Concat Filter concatenates multiple resource streams into a collection of output streams
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class Split : BaseFilter, IFilterValidator
    {
        private const string FilterType = "split";
        private const int DefaultVideoOut = 2;
        private const int FilterMaxInputs = 1;

        public Split() 
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfVideoOut = DefaultVideoOut;
        }
        public Split(int numberOfVideoOut)
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfVideoOut = numberOfVideoOut;
        }

        public int NumberOfVideoOut { get; set; }
        
        public override string ToString()
        {
            return string.Concat(Type, "=", NumberOfVideoOut.ToString(CultureInfo.InvariantCulture));
        }

        #region IFilterValidator
        public bool Validate(Commandv2 command, Filterchainv2 filterchain, List<CommandReceipt> receipts)
        {
            //concat filters should be used independently of other filters
            return filterchain.Filters.Count > 1;
        }
        #endregion
    }
}
