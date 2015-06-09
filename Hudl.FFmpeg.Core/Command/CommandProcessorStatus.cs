namespace Hudl.FFmpeg.Command
{
    /// <summary>
    /// enumeration indicating the state of a command processor
    /// </summary>
    public enum CommandProcessorStatus
    {
        /// <summary>
        /// command processor is closed, cannot recieve commands.
        /// </summary>
        Closed, 
        /// <summary>
        /// command processor is open, and ready to recieve commands.
        /// </summary>
        Ready,
        /// <summary>
        /// command processor is processing a command, and cannot recieve another at this time.
        /// </summary>
        Processing, 
        /// <summary>
        /// command processor is in a faulted state, error logs should be reviewed.
        /// </summary>
        Faulted
    }
}
