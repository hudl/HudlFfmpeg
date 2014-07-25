using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe.Serializers
{
    internal class FFprobeSerializer 
    {
        public static FFprobeSerializerResult Serialize(ICommandProcessor processor)
        {
            if (processor.Status == CommandProcessorStatus.Faulted)
            {
                return null;
            }

            var standardOutputString = processor.StdOut;
            var serializerResult = FFprobeSerializerResult.Create();

            var serializers = new List<IFFprobeSerializer>
                {
                    new FFprobeStreamSerializer(), 
                    new FFprobeFormatSerializer()
                };

            serializers.ForEach(serializer =>
                {
                    var serializerStartIndex = 0;

                    while (serializerStartIndex > -1)
                    {
                        var searchingStartTag = string.Format("[{0}]", serializer.Tag);
                        var searchingEndTag = string.Format("[/{0}]", serializer.Tag);

                        var startTagIndex = standardOutputString.IndexOf(searchingStartTag, serializerStartIndex, StringComparison.OrdinalIgnoreCase);
                        if (startTagIndex == -1)
                        {
                            break;
                        }

                        var endTagIndex = standardOutputString.IndexOf(searchingEndTag, startTagIndex + 1, StringComparison.OrdinalIgnoreCase);
                        if (endTagIndex == -1)
                        {
                            break;
                        }

                        var startAt = startTagIndex + searchingStartTag.Length;
                        var lengthOf = endTagIndex - startAt;
                        var unserializedValueString = standardOutputString.Substring(startAt, lengthOf);

                        var rawSerializedValues = FFprobeGeneralSerializer.Serialize(unserializedValueString);

                        var serializedValues = serializer.Serialize(rawSerializedValues);

                        if (serializedValues != null)
                        {
                            var serializerResultItem = FFprobeSerializerResultItem.Create(serializer.Tag, serializedValues);
                        
                            serializerResult.Results.Add(serializerResultItem);
                        }

                        serializerStartIndex = endTagIndex;
                    }
                });

            return serializerResult;
        }
    }
}
