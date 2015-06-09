using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Filters.Interfaces
{
    /// <summary>
    /// representation of a simple filterchain
    /// </summary>
    public interface IFilterchain
    {
        int InputCount { get; }
        int OutputCount { get; }
        void CreateInput(IStream stream);
        void CreateOutput(IStream stream);
    }
}
