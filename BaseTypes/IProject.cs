using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.BaseTypes
{
    public interface IProject : IFilterable
    {
        IReadOnlyList<IResource> Resources { get; }

        IReadOnlyList<Filtergraph> Filtergraphs { get; }

        TResource Add<TResource>(string path)
            where TResource : IResource, new();

        TResource Add<TResource>(string path, TimeSpan length)
            where TResource : IResource, new();

        TResource Add<TResource>(string path, TimeSpan length, TimeSpan startAt, TimeSpan endAt)
            where TResource : IResource, new();

        TResource Add<TResource>(TResource resource)
            where TResource : IResource, new();

        IProject Remove<TResource>(TResource resource)
            where TResource : IResource, new(); 

        IProject RemoveAt(int index);

        TResource ApplyFilter<TResource, TFilter>()
            where TResource : IResource, new()
            where TFilter : IFilter, new();

        TResource ApplyFilter<TResource, TFilter>(TFilter filter, params IResource[] resources)
            where TResource : IResource, new()
            where TFilter : IFilter, new();

        TResource ApplyFilter<TResource>(Filterchain<TResource> filterchain, params IResource[] resources)
            where TResource : IResource, new();

        TimeSpan GetLength(); 

        ResourceList Render<TProcessor>()
            where TProcessor : ICommandProcessor, new();
    }
}
