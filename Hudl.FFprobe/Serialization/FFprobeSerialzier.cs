using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFprobe.Metadata.Models;
using System.Text.Json;

namespace Hudl.FFprobe.Serialization
{
    public class FFprobeSerializer 
    {
        public static ContainerMetadata Serialize(ICommandProcessor processor)
        {
            if (processor.Status == CommandProcessorStatus.Faulted)
            {
                return null;
            }

            var standardOutputString = processor.StdOut;

            return JsonSerializer.Deserialize<ContainerMetadata>(standardOutputString); 
        }
    }
}
