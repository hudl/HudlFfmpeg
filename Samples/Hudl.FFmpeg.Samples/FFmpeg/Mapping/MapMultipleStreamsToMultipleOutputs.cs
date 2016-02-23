using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using System.Linq;

namespace Hudl.FFmpeg.Samples.FFmpeg.Mapping
{
    public class MapMultipleStreamsToMultipleOutputs
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

            //create a command adding a video and audio file 
            var command = factory.CreateOutputCommand()
                .AddInput(Assets.Utilities.GetVideoFile())
                .AddInput(Assets.Utilities.GetAudioFile());

            //select the first two streams from the input command
            var splitVideo = command.Take(0)
                .Filter(Filterchain.FilterTo<VideoStream>(new Split(2)));
            var splitAudio = command.Take(1)
                .Filter(Filterchain.FilterTo<AudioStream>(new ASplit(2)));

            //map the first audio and video stream to the output
            command.Select(splitVideo.Take(0), splitAudio.Take(0))
                   .MapTo<Mp4>(@"c:\source\ffmpeg\bin\temp\foo.mp4", SettingsCollection.ForOutput(new OverwriteOutput()));
            command.Select(splitVideo.Take(1), splitAudio.Take(1))
                   .MapTo<Mp4>(@"c:\source\ffmpeg\bin\temp\bar.mp4", SettingsCollection.ForOutput(new OverwriteOutput()));

            //render the output
            factory.Render(); 
        }
    }
}
