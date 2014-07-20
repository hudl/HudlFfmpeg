using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public abstract class BaseContainer : IContainer
    {
        protected BaseContainer(string format)
        {
            Format = format;
            Streams = new List<IStream>();
            Id = Guid.NewGuid().ToString();
            Name = string.Concat(Guid.NewGuid(), format);
        }
        protected BaseContainer(string format, string name) 
            : this(format)
        {
            Name = name; 
        }
        protected BaseContainer(string format, string name, string path)
            : this(format, name)
        {
            Path = path;
        }

        /// <summary>
        /// the truly unique id per resource file
        /// </summary>
        public string Id { get; set; }
        
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
        /// list of streams in the container
        /// </summary>
        public List<IStream> Streams { get; set; }

        protected abstract IContainer Clone();

        public IContainer Copy()
        {
            var newInstance = Clone();

            newInstance.Id = Id;
            newInstance.Name = Name;
            newInstance.Path = Path;
            newInstance.Streams = new List<IStream>(Streams);
            newInstance.Streams.ForEach(s => s.Map = Helpers.NewMap());

            return newInstance; 
        }

        public IContainer CreateFrom()
        {
            var instanceNew = Copy();

            Name = string.Concat(Guid.NewGuid(), Format);

            return instanceNew; 
        }

        private bool ValidateFormat(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var upperFormat = Format.ToUpper();
            var upperPath = path.Trim().ToUpper();
            var queryStringIndex = upperPath.IndexOf("?", StringComparison.Ordinal); 
            if (queryStringIndex != -1)
            {
                upperPath = upperPath.Substring(0, queryStringIndex);
            }
            return upperPath.EndsWith(upperFormat);
        }
    }
}
