using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.BaseTypes;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseDuration : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-t";

        protected BaseDuration(TimeSpan length)
            : base(SettingType)
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

        public TimeSpan Length { get; set; }

        public override void Validate()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("Duration length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("Duration length must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Formats.Duration(Length));
        }

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
