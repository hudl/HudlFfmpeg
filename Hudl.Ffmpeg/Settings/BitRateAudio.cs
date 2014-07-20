using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
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
