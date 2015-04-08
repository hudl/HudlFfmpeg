using System.Linq;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFprobe.Command;
using Hudl.FFprobe.Metadata.Models;
using Hudl.FFprobe.Options;
using Hudl.FFprobe.Serialization;

namespace Hudl.FFprobe
{
    public class MediaLoader
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
