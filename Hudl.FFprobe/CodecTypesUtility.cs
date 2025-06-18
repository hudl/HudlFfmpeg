namespace Hudl.FFprobe;

internal static class CodecTypesUtility
{
    public static CodecTypes? GetCodecTypeFromMediaType(string? mediaType) => mediaType?.ToLowerInvariant()?.Trim() switch
    {
        "video" => CodecTypes.Video,
        "audio" => CodecTypes.Audio,
        "data" => CodecTypes.Data,
        _ => null,
    };
}