using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// removes the audio stream from the output file
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "an", IsParameterless = true)]
    public class RemoveAudio : ISetting, IMetadataManipulation
    {
        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.AudioStream = null;

            return infoToUpdate;
        }
    }
}
