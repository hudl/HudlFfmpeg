using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    interface IFilterValidator
    {
        /// <summary>
        /// Validates the Filter based on the command and filterchain logic
        /// </summary>
        /// <param name="command">The command that contains the Filterchain that holds the Filter</param>
        /// <param name="filterchain">The Filterchain that hold the Filter</param>
        /// <param name="resources">The Resource Receipts from the Command that the filter is to be applied against.</param>
        bool Validate(Commandv2 command, Filterchainv2 filterchain, List<CommandReceipt> receipts);
    }
}
