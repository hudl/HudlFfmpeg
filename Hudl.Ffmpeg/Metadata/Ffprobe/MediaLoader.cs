using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Models;
using Hudl.FFmpeg.Metadata.FFprobe.Options;
using Hudl.FFmpeg.Metadata.FFprobe.Serializers;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe
{
    internal class MediaLoader
    {
        public MediaLoader(IContainer resource)
        {
            ReadInfo(resource);
        }

        public void ReadInfo(IContainer resource)
        {
            var ffprobeCommand = FFprobeCommand.Create(resource)
                                               .Register(new PrintFormatJsonOption())
                                               .Register(new ShowFormatOption())
                                               .Register(new ShowStreamsOption())
                                               .Execute();

            var containerMetadata = FFprobeSerializer.Serialize(ffprobeCommand);

            HasAudio = containerMetadata.Streams.OfType<AudioStreamMetadata>().Any();
            HasVideo = containerMetadata.Streams.OfType<VideoStreamMetadata>().Any();
            BaseData = containerMetadata; 
        }

        public bool HasVideo { get; protected set; }
        public bool HasAudio { get; protected set; }
        public ContainerMetadata BaseData { get; protected set; }
    }
}
