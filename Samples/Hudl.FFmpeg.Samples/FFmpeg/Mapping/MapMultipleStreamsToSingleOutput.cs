using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using System.Linq;

namespace Hudl.FFmpeg.Samples.FFmpeg.Mapping
{
    class MapMultipleStreamsToSingleOutput
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

            //OPTION 1: 
            // - this option uses the add input method, which adds the input to the command
            //   but does not add any streams to a stage object. 
            var option1 = factory.CreateOutputCommand()
                .AddInput(Assets.Utilities.GetVideoFile())
                .AddInput(Assets.Utilities.GetAudioFile());

            //select the first two streams from the input command
            var stage = option1.Select(option1.Take(0),
                                       option1.Take(1));

            //map that directly to an output stream
            stage.MapTo<Mp4>(@"c:\source\ffmpeg\bin\temp\foo.mp4", SettingsCollection.ForOutput(new OverwriteOutput()));


            //OPTION 2: 
            // - this option uses the with input method which adds the input to the command
            //   and also adds the streams from the input file to a staging object
            var option2 = factory.CreateOutputCommand()
                .WithInput<VideoStream>(Assets.Utilities.GetVideoFile())
                .WithInput<AudioStream>(Assets.Utilities.GetAudioFile())
                .MapTo<Mp4>(@"c:\source\ffmpeg\bin\temp\bar.mp4", SettingsCollection.ForOutput(new OverwriteOutput()));

            factory.Render();
        }
    }
}
