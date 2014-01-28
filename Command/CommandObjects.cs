using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandObjects
    {
        private CommandObjects(Commandv2 owner)
        {
            Outputs = new List<CommandOutput>();
            Inputs = new List<CommandResourcev2>();
            Filtergraph = Filtergraphv2.Create(owner);
        }

        public static CommandObjects Create(Commandv2 owner)
        {
            return new CommandObjects(owner);
        }

        public Filtergraphv2 Filtergraph { get; internal set; }

        public List<CommandOutput> Outputs { get; internal set; }
        
        public List<CommandResourcev2> Inputs { get; internal set; }

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
