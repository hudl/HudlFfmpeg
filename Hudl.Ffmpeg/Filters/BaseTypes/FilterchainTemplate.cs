using System.Collections.Generic;
using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// This is the base template file for filterchains. This format will contain the necessary base functions for adding and assigning multiple chains for quick functionality.
    /// </summary>
    public abstract class FilterchainTemplate
    {
        public abstract List<StreamIdentifier> SetupTemplate(FfmpegCommand command, List<StreamIdentifier> streamIdList);
    }
}
