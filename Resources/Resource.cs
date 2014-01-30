using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.Resources
{
    /// <summary>
    /// this is a list of resource files used to describe a returned collection of files
    /// </summary>
    public class Resource
    {
        private static List<Type> _videoTypes = new List<Type>();
        private static List<Type> _audioTypes = new List<Type>();
        private static List<Type> _imageTypes = new List<Type>();


        /// <summary>
        /// Creates a new resource with the full path name provided.
        /// </summary>
        public static TResource CreateOutput<TResource>(CommandConfiguration configuration)
            where TResource : class, IResource, new()
        {
            var temporaryResource = new TResource();
            return Create<TResource>(configuration.OutputPath, temporaryResource.FullName, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Creates a new resource with the full path name provided.
        /// </summary>
        public static TResource Create<TResource>(CommandConfiguration configuration)
            where TResource : class, IResource, new()
        {
            var temporaryResource = new TResource();
            return Create<TResource>(configuration.TempPath, temporaryResource.FullName, TimeSpan.FromSeconds(0));
        }

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

        /// <summary>
        /// Creates a new resource with the full path name provided.
        /// </summary>
        public static IVideo VideoFrom(string fullPath)
        {
            return VideoFrom(fullPath, TimeSpan.FromSeconds(0));
        }
        /// <summary>
        /// Creates a new resource with the full path name provided.
        /// </summary>
        public static IAudio AudioFrom(string fullPath)
        {
            return AudioFrom(fullPath, TimeSpan.FromSeconds(0));
        }
        /// <summary>
        /// Creates a new resource with the full path name provided.
        /// </summary>
        public static IImage ImageFrom(string fullPath)
        {
            return ImageFrom(fullPath, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Creates a new resource with the full path name and length provided.
        /// </summary>
        public static IVideo VideoFrom(string fullPath, TimeSpan length)
        {
            var fileName = Helpers.GetNameFromFullName(fullPath);
            var filePath = Helpers.GetPathFromFullName(fullPath);
            return VideoFrom(filePath, fileName, length);
        }
        /// <summary>
        /// Creates a new resource with the full path name and length provided.
        /// </summary>
        public static IAudio AudioFrom(string fullPath, TimeSpan length)
        {
            var fileName = Helpers.GetNameFromFullName(fullPath);
            var filePath = Helpers.GetPathFromFullName(fullPath);
            return AudioFrom(filePath, fileName, length);
        }
        /// <summary>
        /// Creates a new resource with the full path name and length provided.
        /// </summary>
        public static IImage ImageFrom(string fullPath, TimeSpan length)
        {
            var fileName = Helpers.GetNameFromFullName(fullPath);
            var filePath = Helpers.GetPathFromFullName(fullPath);
            return ImageFrom(filePath, fileName, length);
        }

        /// <summary>
        /// Creates a new resource with the file path and name provided.
        /// </summary>
        public static IVideo VideoFrom(string filePath, string fileName)
        {
            return VideoFrom(filePath, fileName, TimeSpan.FromSeconds(0));
        }
        /// <summary>
        /// Creates a new resource with the file path and name provided.
        /// </summary>
        public static IAudio AudioFrom(string filePath, string fileName)
        {
            return AudioFrom(filePath, fileName, TimeSpan.FromSeconds(0));
        }
        /// <summary>
        /// Creates a new resource with the file path and name provided.
        /// </summary>
        public static IImage ImageFrom(string filePath, string fileName)
        {
            return ImageFrom(filePath, fileName, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Creates a new resource with the file path, name, and length provided.
        /// </summary>
        public static IVideo VideoFrom(string filePath, string fileName, TimeSpan length)
        {
            return ResourceFrom<IVideo>(filePath, fileName, length, _videoTypes);
        }
        /// <summary>
        /// Creates a new resource with the file path, name, and length provided.
        /// </summary>
        public static IAudio AudioFrom(string filePath, string fileName, TimeSpan length)
        {
            return ResourceFrom<IAudio>(filePath, fileName, length, _audioTypes);
        }
        /// <summary>
        /// Creates a new resource with the file path, name, and length provided.
        /// </summary>
        public static IImage ImageFrom(string filePath, string fileName, TimeSpan length)
        {
            return ResourceFrom<IImage>(filePath, fileName, length, _imageTypes);
        }

        private static TResource ResourceFrom<TResource>(string filePath, string fileName, TimeSpan length, List<Type> resourceList)
            where TResource : IResource
        {
            var fileExtension = Helpers.GetExtensionFromFullName(fileName);
            if (resourceList.Count == 0)
            {
                resourceList = GetTypes<TResource>();
            }
            var correctResource = resourceList.FirstOrDefault(t => t.Name.ToUpper() == fileExtension.ToUpper());
            if (correctResource == null)
            {
                throw new InvalidOperationException("Cannot derive resource type from path provided.");
            }

            var newInstance = (TResource)Activator.CreateInstance(correctResource);
            newInstance.Path = filePath;
            newInstance.Name = fileName;
            newInstance.Length = length;
            return newInstance;
        }

        private static List<Type> GetTypes<T>()
        {
            // this might break the world
            //LoadReferencedAssemblies();
            // this only works if the assemblies are loaded already :-(
            return AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(a =>
                    {
                        try
                        {
                            return a.GetTypes();

                        }
                        catch (Exception)
                        {
                            return new Type[0];
                        }
                    })
                .Where(t => !t.IsAbstract && typeof(T).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null).ToList();
        }

        
    }
}
