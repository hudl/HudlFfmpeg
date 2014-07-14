using System;
using Hudl.Ffmpeg.Metadata;

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
        /// the file name of the resource that is used
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// the file domain\directory for the resource
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// a readable path for ffmpeg to access 
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// the extension of the file, 
        /// </summary>
        string Format { get; }
        /// <summary>
        /// the ffmpeg resource indicator
        /// </summary>
        string ResourceIndicator { get;  }
        /// <summary>
        /// the metadata information surrounding the resource
        /// </summary>
        MetadataInfo Info { get; set; }
        /// <summary>
        /// method for copying a resource for its base types.
        /// </summary>
        TResource Copy<TResource>()
            where TResource : IResource;

        /// <summary>
        /// method for creating a new resource from the base resource with a new name.
        /// </summary>
        TResource CreateFrom<TResource>()
            where TResource : IResource;
    }
}
