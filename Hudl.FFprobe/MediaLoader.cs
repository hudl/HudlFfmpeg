using System.Linq;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFprobe.Command;
using Hudl.FFprobe.Metadata.Models;
using Hudl.FFprobe.Serialization;
using Hudl.FFprobe.Settings;

namespace Hudl.FFprobe;

public class MediaLoader
{
    public MediaLoader(IContainer resource)
    {
        ReadInfo(resource);
    }

    public void ReadInfo(IContainer resource)
    {
        ReadInfo(resource, LoaderFlags.ShowFormat | LoaderFlags.ShowStreams); 
    }


    public void ReadInfo(IContainer resource, LoaderFlags flags)
    {
        var ffprobeCommand = FFprobeCommand.Create(resource)
            .AddSetting(new PrintFormat(PrintFormat.JsonFormat));

        if (flags.HasFlag(LoaderFlags.ShowFormat))
        {
            ffprobeCommand.AddSetting(new ShowFormat());
        }

        if (flags.HasFlag(LoaderFlags.ShowStreams))
        {
            ffprobeCommand.AddSetting(new ShowStreams());
        }

        if (flags.HasFlag(LoaderFlags.ShowFrames))
        {
            ffprobeCommand.AddSetting(new ShowFrames());
        }
                                               
        var commandProcessor = ffprobeCommand.Execute(null);

        var containerMetadata = FFprobeSerializer.Instance.Serialize(commandProcessor);

        HasAudio = containerMetadata?.Streams?.OfType<AudioStreamMetadata>()?.ToList()?.Count > 0;
        HasVideo = containerMetadata?.Streams?.OfType<VideoStreamMetadata>()?.ToList()?.Count > 0; 
        HasData = containerMetadata?.Streams?.OfType<DataStreamMetadata>()?.ToList()?.Count > 0; 
        HasFrames = containerMetadata?.Frames?.Count > 0; 
        BaseData = containerMetadata;
    }

    public enum LoaderFlags
    {
        None = 0,
        ShowFormat = 1 << 0, 
        ShowStreams = 1 << 1, 
        ShowFrames = 1 << 2,
    }

    public bool HasVideo { get; protected set; }
    public bool HasAudio { get; protected set; }
    public bool HasData { get; protected set; }
    public bool HasFrames { get; protected set; }
    public ContainerMetadata BaseData { get; protected set; }
}