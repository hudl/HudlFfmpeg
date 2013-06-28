using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// representation of a simple filt 
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// the command name for the affect
        /// </summary>
        string Type { get; }

        /// <summary>
        /// maximum number of inputs that the filter can support
        /// </summary>
        int MaxInputs { get; }

        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string ToString();

        /// <summary>
        /// sets up the filter based on the settings in the filterchain
        /// </summary>
        void Setup<TOutput>(Command<TOutput> command, Filterchain<TOutput> filterchain)
            where TOutput : IResource;
    }
}
