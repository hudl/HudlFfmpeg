using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe
{
    internal class FFprobeSerializerResult 
    {
        private FFprobeSerializerResult()
        {
            Results = new List<FFprobeSerializerResultItem>();
        }

        public List<FFprobeSerializerResultItem> Results { get; set; }

        public static FFprobeSerializerResult Create()
        {
            return new FFprobeSerializerResult();
        }

        public int GetCount(FFprobeCodecTypes ffprobeCodecType)
        {
            var streamResultItems = Results.Where(r => r.Type == "STREAM").ToList();
            if (!streamResultItems.Any())
            {
                return 0;
            }

            var codecTypeObject = FFprobeObject.Create(ffprobeCodecType.ToString().ToLower());
            var codecResultItems = Results.Where(r => r.ValuePairs.Any(vp => vp.Key == "codec_type" && vp.Value.Equals(codecTypeObject))).ToList();
            return codecResultItems.Count;
        }

        public IFFprobeValue Get(FFprobeCodecTypes ffprobeCodecType, int streamIndex, string key)
        {
            var streamResultItems = Results.Where(r => r.Type == "STREAM").ToList();
            if (!streamResultItems.Any())
            {
                return null;
            }

            var codecTypeObject = FFprobeObject.Create(ffprobeCodecType.ToString().ToLower());
            var codecResultItems = Results.Where(r => r.ValuePairs.Any(vp => vp.Key == "codec_type" && vp.Value.Equals(codecTypeObject))).ToList(); 
            if (!codecResultItems.Any())
            {
                return null;
            }

            if (codecResultItems.Count <= streamIndex)
            {
                return null;
            }

            var streamData = codecResultItems.ElementAt(streamIndex);
            var streamDataValue = streamData.ValuePairs.FirstOrDefault(vp => vp.Key == key);
            
            return streamDataValue == null ? null : streamDataValue.Value;
        }

        public IFFprobeValue GetFormat(string key)
        {
            var streamResultItems = Results.Where(r => r.Type == "FORMAT").ToList();
            if (!streamResultItems.Any())
            {
                return null;
            }


            var streamData = streamResultItems.First();
            var streamDataValue = streamData.ValuePairs.FirstOrDefault(vp => vp.Key == key);

            return streamDataValue == null ? null : streamDataValue.Value;
        }
    }
}
