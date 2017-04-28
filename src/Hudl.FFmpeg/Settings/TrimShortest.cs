using System.Linq;
using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// The trim shortest setting will trim the output resource to the shortest input resource setting.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "shortest", IsParameterless = true)]
    public class TrimShortest : ISetting
    {
        public MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo)
        {
            return suppliedInfo.OrderBy(supply => (supply.HasAudio)
                ? supply.AudioMetadata.Duration
                : supply.VideoMetadata.Duration).FirstOrDefault();
        }
    }
}
