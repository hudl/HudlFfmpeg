using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// The filter processor interface is used when a filter or template requires input video to be in a specified format. The methods allow the programmer to abstract some of that processing by performing it when the filter is applied.
    /// </summary>
    interface IFilterProcessor
    {
        /// <summary>
        /// Generates a list of Single Input, Single Output prepatory commands, this restraint is enforeced.
        /// </summary>
        /// <typeparam name="TOutput">The command output type</typeparam>
        /// <param name="command">The Command parent that the filter is by association a part of</param>
        /// <param name="filterchain">The Filterchain parent that the filter is part of</param>
        void PrepCommands<TOutput, TResource>(Command<TOutput> command, Filterchain<TResource> filterchain)
            where TOutput : IResource
            where TResource : IResource;
    }
}
