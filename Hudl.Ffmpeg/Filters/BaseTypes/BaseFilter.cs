using System;
using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// The base implementation of Filter's, will store the basic information on setup, and makes it accessible to parents.
    /// </summary>
    public abstract class BaseFilter : IFilter
    {
        protected BaseFilter(string type, int maxInputs)
        {
            Type = type;
            MaxInputs = maxInputs;
        }
    
        /// <summary>
        /// Defines the filter type, name that is given to ffmpeg
        /// </summary>
        public string Type { get; protected set; }

        /// <summary>
        /// Defines the maximum number of allowable inputs to the filter
        /// </summary>
        public int MaxInputs { get; protected set; }

        /// <summary>
        /// Available at [Render] time, In an attempt to allow abstraction in some cases
        /// </summary>
        protected int InputCount { get; set; }

        /// <summary>
        /// Method, called during [Render] to bring forward all the necessary resources, necessary action for maximum abstraction from the user.
        /// </summary>
        /// <param name="command">The command chain the current filter belongs to.</param>
        /// <param name="filterchain">The filterchain that the filter belongs to</param>
        public void Setup(FfmpegCommand command, Filterchain filterchain)
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
