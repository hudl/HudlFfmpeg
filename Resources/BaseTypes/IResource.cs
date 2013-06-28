using System;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    /// <summary>
    /// defines a base implmentation for an input/output file for ffmpege
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// a .NET resource identifier used to define a unique Resource, multiple IResources may have the same ID
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// an ffmpeg representation of the input stream, truly unique, used in identifying the stream further
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
        /// the start time of the clip, by default this will always remain at 
        /// </summary>
        TimeSpan StartAt { get; set; }

        TimeSpan Duration { get; set; }
        /// <summary>
        /// method for copying a resource for its base types.
        /// </summary>
        IResource Copy();
    }
}
