using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.Managers;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Filters.BaseTypes;

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
            FilterchainManager = FiltergraphManager.Create(this);
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

        public FiltergraphManager FilterchainManager { get; set; }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<CommandOutput> Render()
        {
            ExecuteWith<FFmpegCommandProcessor, FFmpegCommandBuilder>();

            return Objects.Outputs; 
        }

        #region Internals
        internal string Id { get; set; }

        internal CommandObjects Objects { get; set; }
        #endregion
    }
}
