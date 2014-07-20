using System.Collections.Generic;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    /// <summary>
    /// defines a base implmentation for an input/output file for ffmpege
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// a .NET resource identifier used to define a unique Resource, multiple IResources may have the same ID
        /// </summary>
        string Id { get; set; }
        
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
        /// list of streams in the container
        /// </summary>
        List<IStream> Streams { get; set; }

        /// <summary>
        /// method for copying a resource for its base types.
        /// </summary>
        IContainer Copy();

        /// <summary>
        /// method for creating a new resource from the base resource with a new name.
        /// </summary>
        IContainer CreateFrom();
    }
}
