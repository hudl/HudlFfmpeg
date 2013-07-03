using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

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
            Resources = new List<CommandResource<IResource>>();
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
        /// Available at [Render] time, brings the resources as available objects to the filters
        /// </summary>
        protected List<CommandResource<IResource>> Resources { get; set; }

        /// <summary>
        /// Method, called during [Render] to bring forward all the necessary resources, necessary action for maximum abstraction from the user.
        /// </summary>
        /// <param name="command">The command chain the current filter belongs to.</param>
        /// <param name="filterchain">The filterchain that the filter belongs to</param>
        public void Setup<TOutput>(Command<TOutput> command, Filterchain<TOutput> filterchain) 
            where TOutput : IResource
        {
            Resources = command.ResourcesFromReceipts(new List<CommandResourceReceipt>(filterchain.Resources));

            if (Resources.Count == 0)
            {
                throw new InvalidOperationException("Cannot setup filter with a resource count of zero.");
            }
            if (Resources.Count > MaxInputs)
            {
                throw new InvalidOperationException("The filter has exceeded the maximum allowed number of inputs.");
            }
        }

        public virtual TimeSpan? LengthDifference
        {
            get { return null; }
        }

        public virtual TimeSpan? LengthOverride
        {
            get { return null; }
        }

        public virtual TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return null;
        }
    }
}
