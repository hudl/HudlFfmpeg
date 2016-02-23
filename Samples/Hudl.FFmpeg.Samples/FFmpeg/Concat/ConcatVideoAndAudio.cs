using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;

namespace Hudl.FFmpeg.Samples.FFmpeg.Concat
{
    class ConcatVideoAndAudio
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

            //create a command adding a two audio and video files
            var command = factory.CreateOutputCommand()
                .AddInput(Assets.Utilities.GetVideoFile())
                .AddInput(Assets.Utilities.GetVideoFile())
                .AddInput(Assets.Utilities.GetAudioFile())
                .AddInput(Assets.Utilities.GetAudioFile());

            //select the first two video streams and run concat filter
            var videoConcat = command.Select(command.Take(0), command.Take(1))
                .Filter(Filterchain.FilterTo<VideoStream>(new Filters.Concat())); 

            //select the first two audio streams and run concat filter
            var audioConcat = command.Select(command.Take(2), command.Take(3))
                .Filter(Filterchain.FilterTo<VideoStream>(new Filters.Concat(1, 0))); 

            command.Select(audioConcat, videoConcat)
                .MapTo<Mp4>(@"c:\source\ffmpeg\bin\temp\foo.mp4", SettingsCollection.ForOutput(new OverwriteOutput()));

            //render the output
            factory.Render();
        }
    }
}
