using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    internal class CommandObjects
    {
        private CommandObjects(FFmpegCommand owner)
        {
            Outputs = new List<CommandOutput>();
            Inputs = new List<CommandInput>();
            Filtergraph = Filtergraph.Create(owner);
        }

        public static CommandObjects Create(FFmpegCommand owner)
        {
            return new CommandObjects(owner);
        }

        public Filtergraph Filtergraph { get; internal set; }

        public List<CommandOutput> Outputs { get; internal set; }
        
        public List<CommandInput> Inputs { get; internal set; }

        public bool ContainsInput(StreamIdentifier streamId)
        {
            return Inputs.Any(input => input.GetStreamIdentifiers().Any(s => s.Equals(streamId))); 
        }
        
        public bool ContainsInput(CommandInput commandInput)
        {
            return Inputs.Any(input => input.Id == commandInput.Id);
        }

        public bool ContainsOutput(CommandOutput commandOutput)
        {
            return Outputs.Any(output => output.Id == commandOutput.Id); 
        }

        public bool ContainsStream(StreamIdentifier streamId)
        {
            return Filtergraph.FilterchainList.Any(f => f.GetStreamIdentifiers().Any(r => r.Equals(streamId)));
        }
    }
}
