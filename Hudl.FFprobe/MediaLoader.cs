using System.Linq;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFprobe.Command;
using Hudl.FFprobe.Metadata.Models;
using Hudl.FFprobe.Serialization;
using Hudl.FFprobe.Settings;

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
                                               .AddSetting(new ShowFormat())
                                               .AddSetting(new ShowStreams())
                                               .AddSetting(new PrintFormat(PrintFormat.JsonFormat))
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
