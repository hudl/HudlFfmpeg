using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Sugar
{
    public static class FiltergraphExtensions
    {
        public static Filtergraph FilterTo<TStreamType>(this Filtergraph filtergraph, params IFilter[] filters)
            where TStreamType : class, IStream, new()
        {
            return filtergraph.FilterTo<TStreamType>(1, filters); 
        }
        public static Filtergraph FilterTo<TStreamType>(this Filtergraph filtergraph, int count, params IFilter[] filters)
            where TStreamType : class, IStream, new()
        {
            var filterchain = Filterchain.FilterTo<TStreamType>(count, filters);

            return filtergraph.Add(filterchain);
        }
    }
}
