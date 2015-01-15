using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Hudl.FFmpeg.Metadata.Ffprobe.BaseTypes.Models
{
    [JsonObject]
    internal class FFprobeResultContainer
    {
        [JsonProperty(PropertyName = "format")]
        public FFprobeResultFormat Format { get; set; }

        [JsonProperty(PropertyName = "streams")]
        public List<FFprobeResultStream> Streams { get; set; }
    }
}
