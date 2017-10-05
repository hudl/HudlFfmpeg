﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hudl.FFmpeg.Command.BaseTypes
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
        /// contains the error message from a faulted state
        /// </summary>
        Exception Error { get; }

        /// <summary>
        /// contains the stdout message from the last command.
        /// </summary>
        string StdOut { get; }

        /// <summary>
        /// contains the last ran Arguments through the processor
        /// </summary>
        string Command { get; }

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

        /// <summary>
        /// processes the given command string against the processor engine
        /// </summary>
        bool Send(string command, CancellationToken token = default(CancellationToken));
    }
}
