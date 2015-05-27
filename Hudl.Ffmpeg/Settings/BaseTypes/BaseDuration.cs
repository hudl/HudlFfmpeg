using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseDuration : ISetting, IMetadataManipulation
    {
        protected BaseDuration(TimeSpan length)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }
        protected BaseDuration(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }

        [SettingValue(Formatter = typeof(TimeSpanFormatter))]
        public TimeSpan Length { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            if (infoToUpdate.HasVideo && infoToUpdate.VideoStream.VideoMetadata.Duration > Length)
            {
                infoToUpdate.VideoStream.VideoMetadata.Duration = Length;
            }
            
            if (infoToUpdate.HasAudio && infoToUpdate.AudioStream.AudioMetadata.Duration > Length)
            {
                infoToUpdate.AudioStream.AudioMetadata.Duration = Length;
            }

            return infoToUpdate; 
        }
    }
}
