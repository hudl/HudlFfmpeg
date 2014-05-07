using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;

namespace Hudl.Ffmpeg.Command
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

            TempPath = outputPath;
            OutputPath = outputPath;
            FfmpegPath = ffmpegPath;
            AssetsPath = assetsPath;
            LoggingAttributes = new Dictionary<string, string>();
            EnvironmentVariables = new Dictionary<string, string>();
        }

        /// <summary>
        /// declares some environmental settings for the command
        /// </summary>
        public Dictionary<string, string> EnvironmentVariables { get; private set; }

        /// <summary>
        /// attributes that are to be included with any log messages through Hudl.Ffmpeg.
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
        /// declares the Ffmpeg path for the command factory, this is where the ffmpeg executable is.
        /// </summary>
        public string FfmpegPath { get; private set; }

        /// <summary>
        /// declares the static resource files path for the command factory, this is where all static resource files will reside.
        /// </summary>
        public string AssetsPath { get; private set; }

        /// <summary>
        /// tells ffmpeg framework whether it should run clean up or not
        /// </summary>
        public bool RunSetup { get; set; }

        /// <summary>
        /// tells ffmpeg framework whether it should run clean up or not
        /// </summary>
        public bool RunCleanup { get; set; }

        /// <summary>
        /// flags that will change the render process behavior
        /// </summary>
        public CommandConfigurationFlagTypes Flags { get; set; }

        public static CommandConfiguration Create(string outputPath, string ffmpegPath)
        {
            return new CommandConfiguration(outputPath, ffmpegPath);   
        }

        public static CommandConfiguration Create(string outputPath, string ffmpegPath, string assetsPath)
        {
            return new CommandConfiguration(outputPath, ffmpegPath, assetsPath);
        }
    }
}
