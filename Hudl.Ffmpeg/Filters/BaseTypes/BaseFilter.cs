using System;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    /// <summary>
    /// The base implementation of Filter's, will store the basic information on setup, and makes it accessible to parents.
    /// </summary>
    public abstract class BaseFilter : IFilter
    {
        /// <summary>
        /// Method, called during [Render] to bring forward all the necessary resources, necessary action for maximum abstraction from the user.
        /// </summary>
        /// <param name="command">The command chain the current filter belongs to.</param>
        /// <param name="filterchain">The filterchain that the filter belongs to</param>
        public void Setup(FFmpegCommand command, Filterchain filterchain)
        {
            InputCount = filterchain.Resources.Count;

            if (InputCount == 0)
            {
                throw new InvalidOperationException("Cannot setup filter with a resource count of zero.");
            }
            if (InputCount > MaxInputs)
            {
                throw new InvalidOperationException("The filter has exceeded the maximum allowed number of inputs.");
            }
        }

        public virtual void Validate()
        {
        }

        public string GetAndValidateString()
        {
            Validate();

            return ToString();
        }
    }
}
