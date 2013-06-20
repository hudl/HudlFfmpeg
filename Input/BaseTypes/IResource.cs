using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Input.BaseTypes
{
    /// <summary>
    /// defines a base implmentation for an input/output file for ffmpege
    /// </summary>
    interface IResource
    {
        /// <summary>
        /// an ffmpeg representation of the input stream 
        /// </summary>
        string Map { get; set; }
        /// <summary>
        /// a unique identifier to distinguish between one input and another
        /// </summary>
        string Id { get; set; } 
        /// <summary>
        /// the path to the video, must be readible path for ffmpeg
        /// </summary>
        string Path { get; set; } 
        /// <summary>
        /// the name of the input video, this can be left blank
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// the duration of the input video, this is used in the processing of time
        /// </summary>
        TimeSpan Duration { get; } 
        /// <summary>
        /// the time at which the video should be processed from and trimmed to
        /// </summary>
        TimeSpan Start { get; set; }
        /// <summary>
        /// the time at which the video should be processed to and trimmed from
        /// </summary>
        TimeSpan End { get; set; }
        /// <summary>
        /// will clone the input instance into a new instance of
        /// </summary>
        IResource Clone();
    }
}
