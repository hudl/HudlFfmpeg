using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    interface IFilterMultiOutput
    {
        /// <summary>
        /// Returns a count for the amount of output streams the filter will result in
        /// </summary>
        int OutputCount();
    }
}
