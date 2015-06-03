using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the audio bit rate for the output stream 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Setting(Name = "b:a")]
    public class BitRateAudio : BaseBitRate, IMetadataManipulation
    {
        public BitRateAudio(int rate)
            : base(rate)
        {
        }
        public BitRateAudio(AudioBitRateType rate)
            : base((int)rate)
        {
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.AudioStream.AudioMetadata.BitRate = Rate * 1000;

            return infoToUpdate;
        }
    }
}
