using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Models;
using Hudl.FFmpeg.Metadata.FFprobe.Options.BaseTypes;
using Newtonsoft.Json;

namespace Hudl.FFmpeg.Metadata.FFprobe.Serializers
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
