using System.Collections.Generic;
using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    interface IFilterValidator
    {
        /// <summary>
        /// Validates the Filter based on the command and filterchain logic
        /// </summary>
        /// <param name="command">The command that contains the Filterchain that holds the Filter</param>
        /// <param name="filterchain">The Filterchain that hold the Filter</param>
        /// <param name="receipts">The Resource Receipts from the Command that the filter is to be applied against.</param>
        bool Validate(FfmpegCommand command, Filterchain filterchain, List<CommandReceipt> receipts);
    }
}
