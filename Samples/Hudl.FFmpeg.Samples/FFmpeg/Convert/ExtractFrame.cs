using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;

namespace Hudl.FFmpeg.Samples.FFmpeg.Convert
{
    class ExtractFrame
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

            //create a command adding a video file
            var command = factory.CreateOutputCommand()
                .AddInput(Assets.Utilities.GetVideoFile(), SettingsCollection.ForInput(new SeekTo(2)))
                .To<Jpg>(@"c:\source\ffmpeg\bin\temp\foo.jpg", SettingsCollection.ForOutput(new VideoFrames(1), new OverwriteOutput())); 

            //render the output
            factory.Render();
        }
    }
}
