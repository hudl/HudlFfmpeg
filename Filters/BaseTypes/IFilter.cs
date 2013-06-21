using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// representation of a simple filt 
    /// </summary>
    interface IFilter
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
    }
}
