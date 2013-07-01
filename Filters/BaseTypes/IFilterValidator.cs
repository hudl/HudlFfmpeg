using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    interface IFilterValidator
    {
        /// <summary>
        /// Validates the Filter based on the command and filterchain logic.
        /// </summary>
        /// <param name="command">The command that contains the Filterchain that holds the Filter</param>
        /// <param name="filterchain">The Filterchain that hold the Filter</param>
        bool Validate(Command<IResource> command, Filterchain<IResource> filterchain);
    }
}
