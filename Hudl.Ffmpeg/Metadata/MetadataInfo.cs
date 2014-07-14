using System;
using System.Drawing;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Metadata
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

        public FfmpegFraction Timebase { get; internal set; }

        public double AspectRatio { get; internal set; }

        public FfmpegFraction FrameRate { get; internal set; }

        public string EncodedApplication { get; internal set; }

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
                    AspectRatio = AspectRatio, 
                    EncodedApplication = EncodedApplication
                };
        }

        internal static MetadataInfo Create()
        {
            return new MetadataInfo();
        }

        internal static MetadataInfo Create(MediaInfo.MediaLoader loader)
        {
            return new MetadataInfo
                {
                    BitRate = loader.BitRate, 
                    Duration = loader.Duration, 
                    HasAudio = loader.HasAudio,
                    HasVideo = loader.HasVideo,
                    AspectRatio = loader.AspectRatio,
                    EncodedApplication = loader.EncodedApplication,
                    Dimensions = new Size(loader.Width, loader.Height),
                };
        }

        internal static MetadataInfo Create(Ffprobe.MediaLoader loader)
        {
            return new MetadataInfo
            {
                BitRate = loader.BitRate,
                Duration = loader.Duration,
                HasAudio = loader.HasAudio,
                HasVideo = loader.HasVideo,
                Timebase = FfmpegFraction.Create(loader.TimeBase),
                FrameRate = FfmpegFraction.Create(loader.FrameRate),
                Dimensions = new Size(loader.Width, loader.Height),
                EncodedApplication = loader.EncodedApplication
            };
        }
    }
}
