using System;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe
{
    internal class FfprobeVideoStream
    {
        private FfprobeVideoStream()
        {
        }

        public int Width { get; set; }

        public int Height { get; set; }
        
        public long BitRate { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public FfprobeFraction TimeBase { get; set; }
        
        public FfprobeFraction FrameRate { get; set; }

        public static FfprobeVideoStream Create()
        {
            return new FfprobeVideoStream();
        }
    }
}
