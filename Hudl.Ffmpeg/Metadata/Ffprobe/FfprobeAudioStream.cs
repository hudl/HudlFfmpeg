using System;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe
{
    internal class FFprobeAudioStream
    {
        private FFprobeAudioStream()
        {
        }

        public long BitRate { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public FFprobeFraction TimeBase { get; set; }

        public static FFprobeAudioStream Create()
        {
            return new FFprobeAudioStream();
        }
    }
}
