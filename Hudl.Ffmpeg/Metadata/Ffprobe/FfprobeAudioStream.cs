using System;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe
{
    internal class FfprobeAudioStream
    {
        private FfprobeAudioStream()
        {
        }

        public long BitRate { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public FfprobeFraction TimeBase { get; set; }

        public static FfprobeAudioStream Create()
        {
            return new FfprobeAudioStream();
        }
    }
}
