using System.Collections.Generic;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    interface IFilterValidator
    {
        /// <summary>
        /// Validates the Filter based on the command and filterchain logic
        /// </summary>
        /// <param name="command">The command that contains the Filterchain that holds the Filter</param>
        /// <param name="filterchain">The Filterchain that hold the Filter</param>
        /// <param name="streamIds">The Resource StreamIdentifiers from the Command that the filter is to be applied against.</param>
        bool Validate(FFmpegCommand command, Filterchain filterchain, List<StreamIdentifier> streamIds);
    }
}
