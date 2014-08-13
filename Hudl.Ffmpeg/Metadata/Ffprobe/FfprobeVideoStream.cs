using System;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe
{
    internal class FFprobeVideoStream
    {
        private FFprobeVideoStream()
        {
        }

        public int Width { get; set; }

        public int Height { get; set; }
        
        public long BitRate { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public FFprobeFraction TimeBase { get; set; }
        
        public FFprobeFraction FrameRate { get; set; }

        public static FFprobeVideoStream Create()
        {
            return new FFprobeVideoStream();
        }
    }
}
