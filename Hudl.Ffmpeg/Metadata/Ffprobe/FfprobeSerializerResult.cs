using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe
{
    internal class FfprobeSerializerResult 
    {
        private FfprobeSerializerResult()
        {
            Results = new List<FfprobeSerializerResultItem>();
        }

        public List<FfprobeSerializerResultItem> Results { get; set; }

        public static FfprobeSerializerResult Create()
        {
            return new FfprobeSerializerResult();
        }

        public int GetCount(FfprobeCodecTypes ffprobeCodecType)
        {
            var streamResultItems = Results.Where(r => r.Type == "STREAM").ToList();
            if (!streamResultItems.Any())
            {
                return 0;
            }

            var codecTypeObject = FfprobeObject.Create(ffprobeCodecType.ToString().ToLower());
            var codecResultItems = Results.Where(r => r.ValuePairs.Any(vp => vp.Key == "codec_type" && vp.Value.Equals(codecTypeObject))).ToList();
            return codecResultItems.Count;
        }

        public IFfprobeValue Get(FfprobeCodecTypes ffprobeCodecType, int streamIndex, string key)
        {
            var streamResultItems = Results.Where(r => r.Type == "STREAM").ToList();
            if (!streamResultItems.Any())
            {
                return null;
            }

            var codecTypeObject = FfprobeObject.Create(ffprobeCodecType.ToString().ToLower());
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

        public IFfprobeValue GetFormat(string key)
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
