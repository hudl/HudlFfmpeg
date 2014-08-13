using Hudl.FFmpeg.Metadata;

namespace Hudl.FFmpeg.Resources.BaseTypes
{
    public interface IStream
    {
        /// <summary>
        /// an ffmpeg representation of the input stream, truly unique, used in identifying the stream further
        /// </summary>
        string Map { get; set; }

        /// <summary>
        /// the ffmpeg resource indicator
        /// </summary>
        string ResourceIndicator { get; }

        /// <summary>
        /// the metadata information surrounding the stream
        /// </summary>
        MetadataInfo Info { get; set; }

        /// <summary>
        /// clones the current stream into a new stream
        /// </summary>
        IStream Copy(); 
    }
}
