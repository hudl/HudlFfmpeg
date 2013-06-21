using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseResource : 
        IResource
    {
        public BaseResource(string format) 
        {
            Map = Guid.NewGuid().ToString();
            _format = format;
            
        }
        public BaseResource(string format, string path) :
            this(format)
        {
            Path = path; 
        }
        public BaseResource(string format, string path, TimeSpan length) :
            this(format, path)
        {
            Length = length; 
        }

        /// <summary>
        /// an ffmpeg representation of the input stream, used in identifying the stream further
        /// </summary>
        public string Map { get; set; }
        /// <summary>
        /// a readable path for ffmpeg to access 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// the extension of the file, 
        /// </summary>
        public string Format { get { return _format; } }
        private string _format; 
        /// <summary>
        /// the duration of the input video, this is used in the processing of time
        /// </summary>
        public TimeSpan Length { get; set; }
        /// <summary>
        /// the time at which the video should be processed from and trimmed to
        /// </summary>
        public TimeSpan StartAt { get; set; }
        /// <summary>
        /// the time at which the video should be processed to and trimmed from
        /// </summary>
        public TimeSpan EndAt { get; set; }
    }
}
