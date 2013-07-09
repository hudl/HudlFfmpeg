using System;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// Represents a configuration for the command factory
    /// </summary>
    public class CommandConfiguration
    {
        public CommandConfiguration(string outputPath, string ffmpegPath, string assetsPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("Output path cannot be empty.", "outputPath");
            }
            if (string.IsNullOrWhiteSpace(ffmpegPath))
            {
                throw new ArgumentException("Ffmpeg path path cannot be empty.", "ffmpegPath");
            }
            if (string.IsNullOrWhiteSpace(assetsPath))
            {
                throw new ArgumentException("Assets path path cannot be empty.", "assetsPath");
            }

            OutputPath = outputPath;
            FfmpegPath = ffmpegPath;
            AssetsPath = assetsPath;
            TempPath = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// declares the temporary output path for the command factory, this is where all non-exported output should go.
        /// </summary>
        public string TempPath { get; private set; }

        /// <summary>
        /// declares the output path for the command factory, this is where all outputs should go.
        /// </summary>
        public string OutputPath { get; private set; }

        /// <summary>
        /// declares the Ffmpeg path for the command factory, this is where the ffmpeg executable is.
        /// </summary>
        public string FfmpegPath { get; private set; }

        /// <summary>
        /// declares the static resource files path for the command factory, this is where all static resource files will reside.
        /// </summary>
        public string AssetsPath { get; private set; }

    }
}
