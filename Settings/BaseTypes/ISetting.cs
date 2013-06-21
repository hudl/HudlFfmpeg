using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// representation of a settomg that can be applied to an Ffmpeg resource
    /// </summary>
    interface ISetting
    {
        /// <summary>
        /// the command name for the affect
        /// </summary>
        string Type { get; }
        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string ToString();
    }
}
