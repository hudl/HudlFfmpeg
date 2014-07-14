using System.Collections.Generic;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class FiltergraphExtensions
    {
        public static Filtergraph FilterTo<TResource>(this Filtergraph filtergraph, params IFilter[] filters)
            where TResource : IResource, new()
        {
            return filtergraph.FilterTo<TResource>(1, filters); 
        }
        public static Filtergraph FilterTo<TResource>(this Filtergraph filtergraph, int count, params IFilter[] filters)
            where TResource : IResource, new()
        {
            var filterchain = Filterchain.FilterTo<TResource>(count, filters);

            return filtergraph.Add(filterchain);
        }

        public static Filtergraph FilterTo(this Filtergraph filtergraph, IResource output, params IFilter[] filters)
        {
            return filtergraph.FilterTo(new List<IResource> {output}, filters);
        }
        public static Filtergraph FilterTo(this Filtergraph filtergraph, List<IResource> outputList, params IFilter[] filters)
        {
            var filterchain = Filterchain.FilterTo(outputList, filters);

            return filtergraph.Add(filterchain);
        }

    }
}
