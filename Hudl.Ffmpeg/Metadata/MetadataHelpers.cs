using System;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Metadata
{
    public class MetadataHelpers
    {
        public static MetadataInfoTreeContainer GetMetadataInfo(FFmpegCommand command, StreamIdentifier streamId)
        {
            //first validate that the streamId does in fact belong to the command. 
            if (!CommandHelper.ReceiptBelongsToCommand(command, streamId))
            {
                throw new ArgumentException("The provided streamId is not part of the provided ffmpeg command.",
                    "streamId");
            }

            var resourceIndex = CommandHelper.IndexOfResource(command, streamId);
            if (resourceIndex > -1)
            {
                return ResourceMetadataInfo(command, resourceIndex);
            }

            var filterchainIndex = CommandHelper.IndexOfFilterchain(command, streamId);
            if (filterchainIndex > -1)
            {
                return FilterchainMetadataInfo(command, filterchainIndex);
            }

            var outputIndex = CommandHelper.IndexOfOutput(command, streamId);
            if (outputIndex > -1)
            {
                return OutputMetadataInfo(command, outputIndex);
            }

            return null;
        }

        internal static MetadataInfoTreeContainer ResourceMetadataInfo(FFmpegCommand command, int index)
        {
            if (command.Inputs.Count <= index)
            {
                return null;
            }

            return ExecuteStreamCalculator(MetadataInfoStreamCalculator.Create(command.Inputs[index]));
        }

        internal static MetadataInfoTreeContainer FilterchainMetadataInfo(FFmpegCommand command, int index)
        {
            if (command.Filtergraph.Count <= index)
            {
                return null;
            }

            return ExecuteStreamCalculator(MetadataInfoStreamCalculator.Create(command, command.Filtergraph[index]));
        }

        internal static MetadataInfoTreeContainer OutputMetadataInfo(FFmpegCommand command, int index)
        {
            if (command.Outputs.Count <= index)
            {
                return null;
            }

            return ExecuteStreamCalculator(MetadataInfoStreamCalculator.Create(command, command.Outputs[index]));
        }

        internal static MetadataInfoTreeContainer ExecuteStreamCalculator(MetadataInfoStreamCalculator streamCalculator)
        {
            streamCalculator.Calculate();

            return streamCalculator.InfoStream.ResultMetadataInfo;
        }
    }
}
