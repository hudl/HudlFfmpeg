using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    /// <summary>
    /// defines a base implmentation for an input/output file for ffmpege
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// an ffmpeg representation of the input stream, used in identifying the stream further
        /// </summary>
        string Map { get; set; }
        /// <summary>
        /// a readable path for ffmpeg to access 
        /// </summary>
        string Path { get; set;  }
        /// <summary>
        /// the extension of the file, 
        /// </summary>
        string Format { get; }
        /// <summary>
        /// the duration of the input video, this is used in the processing of time
        /// </summary>
        TimeSpan Length { get; set; } 
        /// <summary>
        /// the time at which the video should be processed from and trimmed to
        /// </summary>
        TimeSpan StartAt { get; set; }
        /// <summary>
        /// the time at which the video should be processed to and trimmed from
        /// </summary>
        TimeSpan EndAt { get; set; }
    }
}
