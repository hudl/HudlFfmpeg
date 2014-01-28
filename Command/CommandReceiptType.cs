
namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// A receipt indicating the resource that was added to the Command
    /// </summary>
    public enum CommandReceiptType
    {
        /// <summary>
        /// specifies that the receipt is for a command input resource
        /// </summary>
        Input, 
        /// <summary>
        /// specifies that the receipt is for a command output file 
        /// </summary>
        Output, 
        /// <summary>
        /// specifies that the receipt is for a filtergraph stream
        /// </summary>
        Stream
    }
}
