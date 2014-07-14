using System;
using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Metadata
{
    public class MetadataHelpers
    {
        public static MetadataInfo GetMetadataInfo(FfmpegCommand command, CommandReceipt receipt)
        {
            //first validate that the receipt does in fact belong to the command. 
            if (!ReceiptBelongsToCommand(command, receipt))
            {
                throw new ArgumentException("The provided receipt is not part of the provided ffmpeg command.",
                    "receipt");
            }

            var resourceIndex = IndexOfResource(command, receipt);
            if (resourceIndex > -1)
            {
                return ResourceMetadataInfo(command, resourceIndex);
            }

            var filterchainIndex = IndexOfFilterchain(command, receipt);
            if (filterchainIndex > -1)
            {
                return FilterchainMetadataInfo(command, filterchainIndex);
            }

            var outputIndex = IndexOfOutput(command, receipt);
            if (outputIndex > -1)
            {
                return OutputMetadataInfo(command, outputIndex);
            }

            return null;
        }

        internal static bool ReceiptBelongsToCommand(FfmpegCommand command, CommandReceipt receipt)
        {
            return command.Owner.Id == receipt.FactoryId
                   && command.Id == receipt.CommandId;
        }

        internal static int IndexOfFilterchain(FfmpegCommand command, CommandReceipt receipt)
        {
            var matchingFilterchain = command.FilterchainFromReceipt(receipt);
            if (matchingFilterchain == null)
            {
                return -1;
            }

            return command.Filtergraph.IndexOf(matchingFilterchain);
        }

        internal static int IndexOfResource(FfmpegCommand command, CommandReceipt receipt)
        {
            var matchingResource = command.ResourceFromReceipt(receipt);
            if (matchingResource == null)
            {
                return -1;
            }

            return command.Inputs.IndexOf(matchingResource);
        }

        internal static int IndexOfOutput(FfmpegCommand command, CommandReceipt receipt)
        {
            var matchingOutput = command.OutputFromReceipt(receipt);
            if (matchingOutput == null)
            {
                return -1;
            }

            return command.Outputs.IndexOf(matchingOutput);
        }

        internal static MetadataInfo ResourceMetadataInfo(FfmpegCommand command, int index)
        {
            if (command.Inputs.Count <= index)
            {
                return null;
            }

            return ExecuteStreamCalculator(MetadataInfoStreamCalculator.Create(command.Inputs[index]));
        }

        internal static MetadataInfo FilterchainMetadataInfo(FfmpegCommand command, int index)
        {
            if (command.Filtergraph.Count <= index)
            {
                return null;
            }

            return ExecuteStreamCalculator(MetadataInfoStreamCalculator.Create(command, command.Filtergraph[index]));
        }

        internal static MetadataInfo OutputMetadataInfo(FfmpegCommand command, int index)
        {
            if (command.Outputs.Count <= index)
            {
                return null;
            }

            return ExecuteStreamCalculator(MetadataInfoStreamCalculator.Create(command, command.Outputs[index]));
        }

        internal static MetadataInfo ExecuteStreamCalculator(MetadataInfoStreamCalculator streamCalculator)
        {
            streamCalculator.Calculate();

            return streamCalculator.InfoStream.ResultMetadataInfo;
        }
    }
}
