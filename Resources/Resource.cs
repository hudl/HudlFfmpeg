using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.Resources
{
    /// <summary>
    /// this is a list of resource files used to describe a returned collection of files
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Creates a new resource with the full path name provided.
        /// </summary>
        public static TResource Create<TResource>(string fullPath)
            where TResource : class, IResource, new()
        {
            return Create<TResource>(fullPath, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Creates a new resource with the full path name and length provided.
        /// </summary>
        public static TResource Create<TResource>(string fullPath, TimeSpan length)
            where TResource : class, IResource, new()
        {
            var fileName = Helpers.GetNameFromFullName(fullPath);
            var filePath = Helpers.GetPathFromFullName(fullPath);
            return Create<TResource>(filePath, fileName, length);
        }

        /// <summary>
        /// Creates a new resource with the file path and name provided.
        /// </summary>
        public static TResource Create<TResource>(string filePath, string fileName)
            where TResource : class, IResource, new()
        {
            return Create<TResource>(filePath, fileName, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Creates a new resource with the file path, name, and length provided.
        /// </summary>
        public static TResource Create<TResource>(string filePath, string fileName, TimeSpan length)
            where TResource : class, IResource, new()
        {
            return new TResource
                {
                    Name = fileName, 
                    Path = filePath, 
                    Length = length
                };
        }
    }
}
