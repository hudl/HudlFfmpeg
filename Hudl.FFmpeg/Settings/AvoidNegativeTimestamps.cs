using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "avoid_negative_ts")]
    public class AvoidNegativeTimestamps : ISetting, IMetadataManipulation
    {
        public AvoidNegativeTimestamps(AvoidNegativeTimestampsType type)
        {
            Type = type;
        }

        [SettingParameter(Formatter = typeof(EnumParameterFormatter))]
        public AvoidNegativeTimestampsType Type { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            long? videoStartTime = null, audioStartTime = null, videoStartTimeTs = null, 
                  audioStartTimeTs = null, minStartTime = null, minStartTimeTs = null;
            if (infoToUpdate.HasVideo)
            {
                videoStartTime = infoToUpdate.VideoStream.VideoMetadata.StartTime.Ticks;
                videoStartTimeTs = infoToUpdate.VideoStream.VideoMetadata.StartTimeTs;
                minStartTime = videoStartTime;
                minStartTimeTs = videoStartTimeTs; 
            }
            if (infoToUpdate.HasAudio)
            {
                audioStartTime = infoToUpdate.AudioStream.AudioMetadata.StartTime.Ticks;
                audioStartTimeTs = infoToUpdate.AudioStream.AudioMetadata.StartTimeTs;
                minStartTime = audioStartTime;
                minStartTimeTs = audioStartTimeTs;
            }
            if (infoToUpdate.HasVideo && infoToUpdate.HasAudio)
            {
                minStartTime = Math.Min(videoStartTime.Value, audioStartTime.Value);
                minStartTimeTs = Math.Min(videoStartTimeTs.Value, audioStartTimeTs.Value);
            }

            switch (Type)
            {
                case AvoidNegativeTimestampsType.MakeNonNegative:
                    if (minStartTimeTs < 0)
                    {
                        if (infoToUpdate.HasVideo)
                        {
                            infoToUpdate.VideoStream.VideoMetadata.StartTime = TimeSpan.FromTicks(videoStartTime.Value - minStartTime.Value);
                            infoToUpdate.VideoStream.VideoMetadata.StartTimeTs = videoStartTimeTs.Value - minStartTimeTs.Value;
                        }
                        if (infoToUpdate.HasAudio)
                        {
                            infoToUpdate.AudioStream.AudioMetadata.StartTime = TimeSpan.FromTicks(audioStartTime.Value - minStartTime.Value);
                            infoToUpdate.AudioStream.AudioMetadata.StartTimeTs = audioStartTimeTs.Value - minStartTimeTs.Value;
                        }
                    }
                    break;
                case AvoidNegativeTimestampsType.MakeZero:
                    if (minStartTimeTs != 0)
                    {
                        if (infoToUpdate.HasVideo)
                        {
                            infoToUpdate.VideoStream.VideoMetadata.StartTime = TimeSpan.FromTicks(videoStartTime.Value - minStartTime.Value);
                            infoToUpdate.VideoStream.VideoMetadata.StartTimeTs = videoStartTimeTs.Value - minStartTimeTs.Value;
                        }
                        if (infoToUpdate.HasAudio)
                        {
                            infoToUpdate.AudioStream.AudioMetadata.StartTime = TimeSpan.FromTicks(audioStartTime.Value - minStartTime.Value);
                            infoToUpdate.AudioStream.AudioMetadata.StartTimeTs = audioStartTimeTs.Value - minStartTimeTs.Value;
                        }
                    }
                    break;
            }

            return infoToUpdate; 
        }
    }
}
