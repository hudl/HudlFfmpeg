using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hudl.FFprobe.Serialization.Converters;

namespace Hudl.FFprobe.Metadata.Models;

public class FormatMetadata
{
    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("nb_streams")]
    public int NumberOfStreams { get; set; }

    [JsonPropertyName("nb_programs")]
    public int NumberOfPrograms { get; set; }

    [JsonPropertyName("format_name")]
    public string FormatName { get; set; }

    [JsonPropertyName("format_long_name")]
    public string FormatLongName { get; set; }

    [JsonPropertyName("start_time")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan StartTime { get; set; }

    [JsonPropertyName("duration")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Duration { get; set; }

    [JsonPropertyName("size")]
    public string Size { get; set; }

    [JsonPropertyName("bit_rate")]
    public long BitRate { get; set; }

    [JsonPropertyName("probe_score")]
    public int ProbeScore { get; set; }

    [JsonPropertyName("tags")]
    public Dictionary<string, string> Tags { get; set; } 
}