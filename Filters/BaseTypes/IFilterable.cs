namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// interface that forces a type to expose it's filterable interface 
    /// </summary>
    public interface IFilterable
    {
        Filtergraph Filtergraph { get; set; }
    }
}
