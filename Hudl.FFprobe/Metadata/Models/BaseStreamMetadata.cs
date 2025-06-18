using System;
using System.Collections.Generic;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFprobe.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Metadata.Models;

public class BaseStreamMetadata
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("codec_name")]
    public string CodecName { get; set; }

    [JsonPropertyName("codec_long_name")]
    public string CodecLongName { get; set; }

    [JsonPropertyName("codec_type")]
    public string CodecType { get; set; }

    [JsonPropertyName("codec_time_base")]
    [JsonConverter(typeof(FractionConverter))]
    public Fraction CodecTimeBase { get; set; }

    [JsonPropertyName("codec_tag_string")]
    public string CodecTagString { get; set; }

    [JsonPropertyName("codec_tag")]
    public string CodecTag { get; set; }

    [JsonPropertyName("profile")]
    public string Profile { get; set; }

    [JsonPropertyName("time_base")]
    [JsonConverter(typeof(FractionConverter))]
    public Fraction TimeBase { get; set; }

    [JsonPropertyName("bit_rate")]
    public long BitRate { get; set; }

    [JsonPropertyName("start_pts")]
    public long StartTimeTs { get; set; }

    [JsonPropertyName("start_time")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan StartTime { get; set; }

    [JsonPropertyName("duration_ts")]
    public long DurationTs { get; set; }

    [JsonPropertyName("duration")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Duration { get; set; }

    [JsonPropertyName("nb_frames")]
    public int NumberOfFrames { get; set; }

    [JsonPropertyName("disposition")]
    public Dictionary<string, string> Disposition { get; set; } 

    [JsonPropertyName("tags")]
    public Dictionary<string, string> Tags { get; set; }

    [JsonExtensionData] 
    public Dictionary<string, JsonElement> AdditionalData { get; set; } = new();
}