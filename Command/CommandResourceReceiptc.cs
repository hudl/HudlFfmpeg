
namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// A receipt indicating the resource that was added to the Command
    /// </summary>
    public class CommandResourceReceipt
    {
        internal CommandResourceReceipt(string factoryId, string commandId, string map)
        {
            Map = map;
            CommandId = commandId;
            FactoryId = factoryId;
        }

        /// <summary>
        /// referece to the command resource file
        /// </summary>
        public string Map { get; protected set; }

        /// <summary>
        /// reference to the command that holds this reference
        /// </summary>
        public string CommandId { get; protected set; }

        /// <summary>
        /// reference to the command factory that holds this reference
        /// </summary>
        public string FactoryId { get; protected set; }
    }
}
