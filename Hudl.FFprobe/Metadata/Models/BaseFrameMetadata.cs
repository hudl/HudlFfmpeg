using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFprobe.Serialization.Converters;

namespace Hudl.FFprobe.Metadata.Models;

public class BaseFrameMetadata
{
    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }

    [JsonPropertyName("stream_index")]
    public int StreamIndex { get; set; }

    [JsonPropertyName("key_frame")]
    public long KeyFrame { get; set; }

    [JsonPropertyName("pkt_pts")]
    public long PacketPresentationTimestamp { get; set; }

    [JsonPropertyName("pkt_pts_time")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan PacketPresentationTimestampTime { get; set; }

    [JsonPropertyName("pkt_dts")]
    public long PacketDecodingTimestamp { get; set; }

    [JsonPropertyName("pkt_dts_time")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan PacketDecodingTimestampTime { get; set; }

    [JsonPropertyName("pkt_duration")]
    public long PacketDuration { get; set; }

    [JsonPropertyName("pkt_duration_time")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan PacketDurationTime { get; set; }

    [JsonPropertyName("best_effort_timestamp")]
    public long BestEffortTimestamp { get; set; }

    [JsonPropertyName("best_effort_timestamp_time")]
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan BestEffortTimestampTime { get; set; }

    [JsonExtensionData] 
    public Dictionary<string, JsonElement> AdditionalData { get; set; } = new();
}