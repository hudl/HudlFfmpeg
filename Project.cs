using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.Templates
{
    public class Project : IProject
    {
        private new List<IResource> _resources;
        public IReadOnlyList<IResource> Resources { get { return _resources.AsReadOnly(); } }

        public TypeA Add<TypeA>() where TypeA : IResource, new()
        {
            return Add(new TypeA()); 
        }

        public TypeA Add<TypeA>(string path) where TypeA : IResource, new()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Resource path cannot be null or empty.", "path"); 

            return Add(new TypeA()
            {
                Path = path
            });
        }

        public TypeA Add<TypeA>(string path, TimeSpan length) where TypeA : IResource, new()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Resource path cannot be null or empty.", "path");
            if (length == null)
                throw new ArgumentException("Resource length cannot be null or empty.", "length");

            return Add(new TypeA()
            {
                Path = path, 
                Length = length
            });
        }

        public TypeA Add<TypeA>(string path, TimeSpan length, TimeSpan startAt, TimeSpan endAt) where TypeA : IResource, new()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Resource path cannot be null or empty.", "path");
            if (length == null)
                throw new ArgumentException("Resource length cannot be null or empty.", "length");
            if (startAt == null)
                throw new ArgumentException("Resource starting time cannot be null or empty.", "startAt");
            if (endAt == null)
                throw new ArgumentException("Resource ending time cannot be null or empty.", "endAt");

            return Add(new TypeA()
            {
                Path = path,
                Length = length, 
                StartAt = startAt,
                EndAt = endAt
            });
        }
       
        public TypeA Add<TypeA>(TypeA resource) where TypeA : IResource, new()
        {
            if (resource == null)
                throw new ArgumentException("Provided resource file cannot be null.", "resource");
            if (Contains(resource))
                throw new ArgumentException("Provided resource file is already part of the project.", "resource");

            _resources.Add(resource);
            return resource;
        }

        public bool Contains<TypeA>(TypeA resource) where TypeA : IResource, new()
        {
            if (resource == null)
                throw new ArgumentException("Provided resource file cannot be null.", "resource");
            //return _resources.Find(r => 
        }

        public IProject Remove<TypeA>(TypeA resource) where TypeA : IResource, new()
        {
            throw new NotImplementedException();
        }

        public IProject RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetLength()
        {
            throw new NotImplementedException();
        }

        public ResourceList Render<TypeP>() where TypeP : Command.BaseTypes.ICommandProcessor, new()
        {
            throw new NotImplementedException();
        }

        public Filters.BaseTypes.Filtergraph Filtergraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public IReadOnlyList<Filters.BaseTypes.Filtergraph> Filtergraphs
        {
            get { throw new NotImplementedException(); }
        }

        public TResource ApplyFilter<TResource, TFilter>()
            where TResource : IResource, new()
            where TFilter : Filters.BaseTypes.IFilter, new()
        {
            throw new NotImplementedException();
        }

        public TResource ApplyFilter<TResource, TFilter>(TFilter filter, params IResource[] resources)
            where TResource : IResource, new()
            where TFilter : Filters.BaseTypes.IFilter, new()
        {
            throw new NotImplementedException();
        }

        public TResource ApplyFilter<TResource>(Filters.BaseTypes.Filterchain<TResource> filterchain, params IResource[] resources) where TResource : IResource, new()
        {
            throw new NotImplementedException();
        }
    }
}
