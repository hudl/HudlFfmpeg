using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Samples.Assets;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.FFmpeg.Samples.Metadata
{
    class MetadataCalculation
    {
        public static void Run()
        {
            //set up ffmpeg command configuration with locations of default output folders, ffmpeg, and ffprobe.
            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(
                @"c:\source\ffmpeg\bin\temp",
                @"c:\source\ffmpeg\bin\ffmpeg.exe",
                @"c:\source\ffmpeg\bin\ffprobe.exe");

            //create a factory 
            var factory = CommandFactory.Create();

            //set up the output and input settings 
            var inputSettings = SettingsCollection.ForInput(new SeekPositionInput(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000),
                new DurationOutput(10d));

            //create a command adding a two audio files
            var command = factory.CreateOutputCommand(); 

            var output = command.WithInput<VideoStream>(Utilities.GetVideoFile(), inputSettings)
                .To<Mp4>(@"c:\source\ffmpeg\bin\temp\foo.mp4", outputSettings)
                .First();

            //use the metadata helper calculation engine to determine a snapshot of what the output duration should be.
            var metadataInfo = MetadataHelpers.GetMetadataInfo(command, output.GetStreamIdentifier());

            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Metadata snapshot:");
            Console.WriteLine(string.Format("Duration: {0}", metadataInfo.VideoStream.VideoMetadata.Duration));
            Console.WriteLine(string.Format("Has Video: {0}", metadataInfo.HasVideo));
            Console.WriteLine(string.Format("Has Audio: {0}", metadataInfo.HasAudio));
            System.Console.ForegroundColor = ConsoleColor.White; 
        }
    }
}
