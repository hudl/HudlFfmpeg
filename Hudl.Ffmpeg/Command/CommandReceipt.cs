
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// A receipt indicating the resource that was added to the Command
    /// </summary>
    public class CommandReceipt
    {
        private CommandReceipt(string factoryId, string commandId, string map, CommandReceiptType type)
        {
            Map = map;
            Type = type;
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

        /// <summary>
        /// determines if the receipt is an input or output receipt
        /// </summary>
        public CommandReceiptType Type { get; protected set; }

        public bool Equals(CommandReceipt receipt)
        {
            return Type == receipt.Type
                   && Map == receipt.Map
                   && CommandId == receipt.CommandId
                   && FactoryId == receipt.FactoryId;
        }

        #region Internals 
        internal static CommandReceipt CreateFromInput(string factoryId, string commandId, string map)
        {
            return new CommandReceipt(factoryId, commandId, map, CommandReceiptType.Input);    
        }
        internal static CommandReceipt CreateFromOutput(string factoryId, string commandId, string map)
        {
            return new CommandReceipt(factoryId, commandId, map, CommandReceiptType.Output);    
        }
        internal static CommandReceipt CreateFromStream(string factoryId, string commandId, string map)
        {
            return new CommandReceipt(factoryId, commandId, map, CommandReceiptType.Stream);    
        }
        #endregion 
    }
}
