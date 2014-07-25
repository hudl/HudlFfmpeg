using System;
using System.Drawing;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata.FFprobe;

namespace Hudl.FFmpeg.Metadata
{
    public class MetadataInfo
    {
        private MetadataInfo()
        {
            Dimensions = new Size(0,0);
            Duration = TimeSpan.FromSeconds(0); 
        }

        public Size Dimensions { get; internal set; }

        public TimeSpan Duration { get; internal set; }

        public long BitRate { get; internal set; }

        public bool HasAudio { get; internal set; }

        public bool HasVideo { get; internal set; }

        public FFmpegFraction Timebase { get; internal set; }


        public FFmpegFraction FrameRate { get; internal set; }


        public MetadataInfo Copy()
        {
            return new MetadataInfo
                {
                    Dimensions = Dimensions, 
                    Duration = Duration, 
                    BitRate = BitRate,
                    HasAudio = HasAudio,
                    HasVideo = HasVideo,
                    Timebase = Timebase,
                    FrameRate = FrameRate,
                };
        }

        internal static MetadataInfo Create()
        {
            return new MetadataInfo();
        }

        internal static MetadataInfo Create(FFprobeAudioStream loader)
        {
            return new MetadataInfo
            {
                HasAudio = true,
                HasVideo = false,
                BitRate = loader.BitRate,
                Duration = loader.Duration,
                Timebase = FFmpegFraction.Create(loader.TimeBase),
            };
        }

        internal static MetadataInfo Create(FFprobeVideoStream loader)
        {
            return new MetadataInfo
            {
                HasVideo = true,
                HasAudio = false,
                BitRate = loader.BitRate,
                Duration = loader.Duration,
                Timebase = FFmpegFraction.Create(loader.TimeBase),
                FrameRate = FFmpegFraction.Create(loader.FrameRate),
                Dimensions = new Size(loader.Width, loader.Height),
            };
        }
    }
}
