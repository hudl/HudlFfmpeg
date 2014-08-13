using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the audio bit rate for the output stream 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitRateAudio : BaseBitRate, IMetadataManipulation
    {
        private const string Suffix = ":a";

        public BitRateAudio(int rate)
            : base(Suffix, rate)
        {
        }
        public BitRateAudio(AudioBitRateType rate)
            : base(Suffix, (int)rate)
        {
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.AudioStream.BitRate = Rate * 1000;

            return infoToUpdate;
        }
    }
}
