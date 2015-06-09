using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFprobe.Metadata.Models;
using Newtonsoft.Json;

namespace Hudl.FFprobe.Serialization
{
    internal class FFprobeSerializer 
    {
        public static ContainerMetadata Serialize(ICommandProcessor processor)
        {
            if (processor.Status == CommandProcessorStatus.Faulted)
            {
                return null;
            }

            var standardOutputString = processor.StdOut;

            return JsonConvert.DeserializeObject<ContainerMetadata>(standardOutputString); 
        }
    }
}
