using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandObjects
    {
        private CommandObjects(FfmpegCommand owner)
        {
            Outputs = new List<CommandOutput>();
            Inputs = new List<CommandResource>();
            Filtergraph = Filtergraph.Create(owner);
        }

        public static CommandObjects Create(FfmpegCommand owner)
        {
            return new CommandObjects(owner);
        }

        public Filtergraph Filtergraph { get; internal set; }

        public List<CommandOutput> Outputs { get; internal set; }
        
        public List<CommandResource> Inputs { get; internal set; }

        public bool ContainsInput(CommandReceipt receipt)
        {
            return receipt.Type == CommandReceiptType.Input && Inputs.Any(input => input.GetReceipt().Equals(receipt));
        }

        public bool ContainsOutput(CommandReceipt receipt)
        {
            return receipt.Type == CommandReceiptType.Output && Outputs.Any(output => output.GetReceipt().Equals(receipt));
        }

        public bool ContainsStream(CommandReceipt receipt)
        {
            return receipt.Type == CommandReceiptType.Stream && Filtergraph.FilterchainList.Any(f => f.GetReceipts().Any(r => r.Equals(receipt)));
        }
    }
}
