using System;
using System.IO;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseResource : IResource
    {
        protected BaseResource(string format)
        {
            Format = format;
            Id = Guid.NewGuid().ToString();
            Map = Guid.NewGuid().ToString();
            Path = string.Concat(Guid.NewGuid(), format);
        }
        protected BaseResource(string format, string path) 
            : this(format)
        {
            Path = path; 
        }
        protected BaseResource(string format, string path, TimeSpan length) 
            : this(format, path)
        {
            Length = length; 
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
        /// a readable path for ffmpeg to access 
        /// </summary>
        public string Path 
        { 
            get { return _path; }
            set
            {
                if (!ValidateFormat(value))
                {
                    throw new ArgumentException(string.Format(
                        "Path must have an extension of '{0}' for this resource.", Format));
                }
                _path = value;
            }
        }
        private string _path; 

        /// <summary>
        /// the extension of the file, 
        /// </summary>
        public string Format { get; protected set; }
        
        /// <summary>
        /// the duration of the input video, this is used in the processing of time
        /// </summary>
        public TimeSpan Length { get; set; }
        
        /// <summary>
        /// copies the current iresource as a seperate but equal type
        /// </summary>
        public abstract IResource Copy();
        
        /// <summary>
        /// abstracted method for copying a resource.
        /// </summary>
        protected TResource Copy<TResource>()
            where TResource : IResource, new()
        {
            return new TResource
                {
                    Id = Id,
                    Path = Path, 
                    Length = Length,
                    Map = Guid.NewGuid().ToString()
                };
        }


        private bool ValidateFormat(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.Trim().ToUpper().EndsWith(Format.Trim().ToUpper());
        }
    }
}
