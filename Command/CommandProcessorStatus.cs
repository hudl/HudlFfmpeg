using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// enumeration indicating the state of a command processor
    /// </summary>
    enum CommandProcessorStatus
    {
        /// <summary>
        /// command processor is closed, cannot recieve commands.
        /// </summary>
        closed, 
        /// <summary>
        /// command processor is open, and ready to recieve commands.
        /// </summary>
        ready,
        /// <summary>
        /// command processor is processing a command, and cannot recieve another at this time.
        /// </summary>
        processing, 
        /// <summary>
        /// command processor is in a faulted state, error logs should be reviewed.
        /// </summary>
        faulted
    }
}
