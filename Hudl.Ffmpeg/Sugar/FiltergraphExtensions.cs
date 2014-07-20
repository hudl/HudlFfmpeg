using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
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
