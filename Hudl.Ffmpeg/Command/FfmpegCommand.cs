using System;
using System.Collections.ObjectModel;
using System.Deployment.Internal;
using System.Linq;
using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Managers;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    public class FFmpegCommand : FFcommandBase
    {
        private FFmpegCommand(CommandFactory owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;
            Id = Guid.NewGuid().ToString();
            Objects = CommandObjects.Create(this);
            OutputManager = CommandOutputManager.Create(this);
            InputManager = CommandInputManager.Create(this);
            FilterchainManager = CommandFiltergraphManager.Create(this);
        }

        public static FFmpegCommand Create(CommandFactory owner)
        {
            return new FFmpegCommand(owner);    
        }

        public ReadOnlyCollection<CommandOutput> Outputs { get { return Objects.Outputs.AsReadOnly(); } }

        public ReadOnlyCollection<CommandInput> Inputs { get { return Objects.Inputs.AsReadOnly(); } }

        public ReadOnlyCollection<Filterchain> Filtergraph { get { return Objects.Filtergraph.FilterchainList.AsReadOnly(); } }

        public CommandOutputManager OutputManager { get; set; }

        public CommandInputManager InputManager { get; set; }

        public CommandFiltergraphManager FilterchainManager { get; set; }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<CommandOutput> Render()
        {
            //TODO: fix
            //ExecuteWith<FFmpegProcessorReciever>();

            return Objects.Outputs; 
        }

        #region Internals
        internal string Id { get; set; }

        internal CommandObjects Objects { get; set; }
        #endregion
    }
}
