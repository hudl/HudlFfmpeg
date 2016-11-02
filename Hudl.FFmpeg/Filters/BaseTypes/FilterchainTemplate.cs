using System.Collections.Generic;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    /// <summary>
    /// This is the base template file for filterchains. This format will contain the necessary base functions for adding and assigning multiple chains for quick functionality.
    /// </summary>
    public abstract class FilterchainTemplate
    {
        public abstract List<StreamIdentifier> SetupTemplate(FFmpegCommand command, List<StreamIdentifier> streamIdList);
    }
}
