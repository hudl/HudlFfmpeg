using System.Text.Json;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFprobe.Metadata.Models;
using Hudl.FFprobe.Serialization.Converters;

namespace Hudl.FFprobe.Serialization;

public class FFprobeSerializer
{
    private static FFprobeSerializer? _instance = null;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    private FFprobeSerializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.Converters.Add(new ShortNumberConverter());
        _jsonSerializerOptions.Converters.Add(new IntNumberConverter());
        _jsonSerializerOptions.Converters.Add(new LongNumberConverter());
        _jsonSerializerOptions.Converters.Add(new NumberStringConverter());
    }
    
    public static FFprobeSerializer Instance => _instance ??= new FFprobeSerializer();
    public ContainerMetadata? Serialize(ICommandProcessor processor)
    {
        if (processor.Status == CommandProcessorStatus.Faulted)
        {
            return null;
        }

        var standardOutputString = processor.StdOut;

        
        return JsonSerializer.Deserialize<ContainerMetadata>(standardOutputString, _jsonSerializerOptions); 
    }
}