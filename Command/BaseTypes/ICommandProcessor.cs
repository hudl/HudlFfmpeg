using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Command.BaseTypes
{
    /// <summary>
    /// Used in the aide of processing projects, allows the output to several formats
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// contains the current state of the command processor.
        /// </summary>
        CommandProcessorStatus Status { get; } 
        /// <summary>
        /// opens a command builder session, should get the processor started and ready to recieve commands
        /// </summary>
        bool Open();
        /// <summary>
        /// closes a command builder session, should perform all necessary clean up and stop listening for commands
        /// </summary>
        bool Close();
        /// <summary>
        /// processes the given command string against the processor engine
        /// </summary>
        bool Send(string command);
    }
}
