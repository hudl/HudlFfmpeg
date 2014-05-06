using System;
using System.Drawing;
using Hudl.Ffmpeg.MediaInfo;

namespace Hudl.Ffmpeg.Resources.BaseTypes
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

        public double AspectRatio { get; internal set; }

        public static MetadataInfo Create()
        {
            return new MetadataInfo();
        }
        public static MetadataInfo Create(MediaLoader loader)
        {
            return new MetadataInfo
                {
                    BitRate = loader.BitRate, 
                    Duration = loader.Duration, 
                    HasAudio = loader.HasAudio,
                    HasVideo = loader.HasVideo,
                    AspectRatio = loader.AspectRatio,
                    Dimensions = new Size(loader.Width, loader.Height),
                };
        }
    }
}
