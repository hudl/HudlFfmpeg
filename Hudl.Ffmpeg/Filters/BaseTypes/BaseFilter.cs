using System;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    /// <summary>
    /// The base implementation of Filter's, will store the basic information on setup, and makes it accessible to parents.
    /// </summary>
    public abstract class BaseFilter : IFilter
    {
        protected BaseFilter(string k, int ks)
        {
            
        }
        /// <summary>
        /// Method, called during [Render] to bring forward all the necessary resources, necessary action for maximum abstraction from the user.
        /// </summary>
        /// <param name="command">The command chain the current filter belongs to.</param>
        /// <param name="filterchain">The filterchain that the filter belongs to</param>
        public void Setup(FFmpegCommand command, Filterchain filterchain)
        {
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
