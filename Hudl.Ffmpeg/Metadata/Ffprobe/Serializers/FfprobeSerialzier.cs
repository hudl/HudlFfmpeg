using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe.Serializers
{
    internal class FfprobeSerializer 
    {
        public static FfprobeSerializerResult Serialize(ICommandProcessor processor)
        {
            if (processor.Status == CommandProcessorStatus.Faulted)
            {
                return null;
            }

            var standardOutputString = processor.StdOut;
            var serializerResult = FfprobeSerializerResult.Create();

            var serializers = new List<IFfprobeSerializer>
                {
                    new FfprobeStreamSerializer(), 
                    new FfprobeFormatSerializer()
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

                        var rawSerializedValues = FfprobeGeneralSerializer.Serialize(unserializedValueString);

                        var serializedValues = serializer.Serialize(rawSerializedValues);

                        if (serializedValues != null)
                        {
                            var serializerResultItem = FfprobeSerializerResultItem.Create(serializer.Tag, serializedValues);
                        
                            serializerResult.Results.Add(serializerResultItem);
                        }

                        serializerStartIndex = endTagIndex;
                    }
                });

            return serializerResult;
        }
    }
}
