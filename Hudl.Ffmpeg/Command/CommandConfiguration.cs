using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    /// <summary>
    /// Represents a configuration for the command factory
    /// </summary>
    public class CommandConfiguration
    {
        public CommandConfiguration(string outputPath, string ffmpegPath)
            : this(outputPath, ffmpegPath, outputPath)
        {
        }
        public CommandConfiguration(string outputPath, string ffmpegPath, string assetsPath)
            : this(outputPath, ffmpegPath, string.Empty, assetsPath)
        {
        }
        public CommandConfiguration(string outputPath, string ffmpegPath, string ffprobePath, string assetsPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("Output path cannot be empty.", "outputPath");
            }
            if (string.IsNullOrWhiteSpace(ffmpegPath))
            {
                throw new ArgumentException("FFmpeg path path cannot be empty.", "ffmpegPath");
            }
            if (string.IsNullOrWhiteSpace(assetsPath))
            {
                throw new ArgumentException("Assets path path cannot be empty.", "assetsPath");
            }

            TempPath = outputPath;
            OutputPath = outputPath;
            AssetsPath = assetsPath;
            FFmpegPath = ffmpegPath;
            FFprobePath = ffprobePath;
            LoggingAttributes = new Dictionary<string, string>();
            EnvironmentVariables = new Dictionary<string, string>();
        }


        /// <summary>
        /// declares some environmental settings for the command
        /// </summary>
        public Dictionary<string, string> EnvironmentVariables { get; private set; }

        /// <summary>
        /// attributes that are to be included with any log messages through Hudl.FFmpeg.
        /// </summary>
        public Dictionary<string, string> LoggingAttributes { get; set; } 

        /// <summary>
        /// declares the temporary output path for the command factory, this is where all non-exported output should go.
        /// </summary>
        public string TempPath { get; private set; }

        /// <summary>
        /// declares the output path for the command factory, this is where all outputs should go.
        /// </summary>
        public string OutputPath { get; private set; }

        /// <summary>
        /// declares the FFmpeg path for the command factory, this is where the ffmpeg executable is.
        /// </summary>
        public string FFmpegPath { get; private set; }

        /// <summary>
        /// declares the FFprobe path for the command factory, this is where the FFprobe executable is.
        /// </summary>
        public string FFprobePath { get; private set; }

        /// <summary>
        /// declares the static resource files path for the command factory, this is where all static resource files will reside.
        /// </summary>
        public string AssetsPath { get; private set; }

        /// <summary>
        /// flags that will change the render process behavior
        /// </summary>
        public CommandConfigurationFlagTypes Flags { get; set; }

        public static CommandConfiguration Create(string outputPath, string ffmpegPath)
        {
            return new CommandConfiguration(outputPath, ffmpegPath);   
        }

        public static CommandConfiguration Create(string outputPath, string ffmpegPath, string ffprobePath)
        {
            return new CommandConfiguration(outputPath, ffmpegPath, ffprobePath, outputPath);
        }

    }
}
