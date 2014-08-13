using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Filters.BaseTypes
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
        /// A filter must contain a validation system for stringification
        /// </summary>
        void Validate();
        
        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string GetAndValidateString(); 

        /// <summary>
        /// sets up the filter based on the settings in the filterchain
        /// </summary>
        void Setup(FFmpegCommand command, Filterchain filterchain);
    }
}
