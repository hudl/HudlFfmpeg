using System;
using System.IO;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseResource : IResource
    {
        protected BaseResource(string format, string resourceIndicator)
        {
            Format = format;
            Id = Guid.NewGuid().ToString();
            Map = Helpers.NewMap();
            ResourceIndicator = resourceIndicator;
            Name = string.Concat(Guid.NewGuid(), format);
        }
        protected BaseResource(string format, string resourceIndicator, string name) 
            : this(format, resourceIndicator)
        {
            Name = name; 
        }
        protected BaseResource(string format, string resourceIndicator, string name, string path)
            : this(format, resourceIndicator, name)
        {
            Path = path;
        }
        protected BaseResource(string format, string resourceIndicator, string name, TimeSpan length) 
            : this(format, resourceIndicator, name)
        {
            Length = length; 
        }
        protected BaseResource(string format, string resourceIndicator, string name, string path, TimeSpan length)
            : this(format, resourceIndicator, name, length)
        {
            Path = path;
        }

        /// <summary>
        /// the truly unique id per resource file
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// an ffmpeg representation of the input stream, used in identifying the stream further
        /// </summary>
        public string Map { get; set; }
        
        /// <summary>
        /// the file name of the resource that is used
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!ValidateFormat(value))
                {
                    throw new ArgumentException(string.Format(
                        "Name must have an extension of '{0}' for this resource.", Format));
                }
                _name = value;
            }
        }
        private string _name;
        
        /// <summary>
        /// the file domain\directory for the resource
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// a readable path for ffmpeg to access 
        /// </summary>
        public string FullName
        {
            get
            {
                return System.IO.Path.Combine(Path, Name);
            }
        }

        /// <summary>
        /// the extension of the file, 
        /// </summary>
        public string Format { get; protected set; }

        /// <summary>
        /// the ffmpeg resource indicator
        /// </summary>
        public string ResourceIndicator { get; private set; }

        /// <summary>
        /// the duration of the input video, this is used in the processing of time
        /// </summary>
        public TimeSpan Length { get; set; }
        
        /// <summary>
        /// method for copying a resource.
        /// </summary>
        public TResource Copy<TResource>()
            where TResource : IResource
        {
            var instanceNew = InstanceOfMe();
            instanceNew.Map = Helpers.NewMap();
            return (TResource)instanceNew; 
        }

        public TResource CreateFrom<TResource>() 
            where TResource : IResource
        {
            var instanceNew = Copy<TResource>();
            Name = string.Concat(Guid.NewGuid(), Format);
            return instanceNew; 
        }

        protected abstract IResource InstanceOfMe();

        private bool ValidateFormat(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.Trim().ToUpper().EndsWith(Format.Trim().ToUpper());
        }





       
    }
}
