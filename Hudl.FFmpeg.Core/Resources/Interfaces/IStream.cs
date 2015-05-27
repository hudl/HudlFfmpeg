namespace Hudl.FFmpeg.Resources.Interfaces
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
        /// clones the current stream into a new stream
        /// </summary>
        IStream Copy(); 
    }
}
