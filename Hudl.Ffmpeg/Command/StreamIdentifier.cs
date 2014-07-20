
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// A streamId indicating the resource that was added to the Command
    /// </summary>
    public class StreamIdentifier
    {
        private StreamIdentifier(string factoryId, string commandId, string map)
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

        public bool Equals(StreamIdentifier streamId)
        {
            return Map == streamId.Map
                   && CommandId == streamId.CommandId
                   && FactoryId == streamId.FactoryId;
        }

        #region Internals 
        internal static StreamIdentifier Create(string factoryId, string commandId, string map)
        {
            return new StreamIdentifier(factoryId, commandId, map);    
        }
        #endregion 
    }
}
