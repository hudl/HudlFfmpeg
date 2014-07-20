namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// A streamId indicating the resource that was added to the Command
    /// </summary>
    public enum CommandReceiptType
    {
        /// <summary>
        /// specifies that the streamId is for a command input resource
        /// </summary>
        Input, 
        /// <summary>
        /// specifies that the streamId is for a command output file 
        /// </summary>
        Output, 
        /// <summary>
        /// specifies that the streamId is for a filtergraph stream
        /// </summary>
        Stream
    }
}
