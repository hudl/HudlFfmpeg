using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;

namespace Hudl.FFmpeg.Samples.FFmpeg.Events
{
    class AttachOnSuccessOnErrorEvents
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
                .AddInput(Assets.Utilities.GetVideoFile())
                .OnSuccess(OnSuccessAction)
                .OnError(OnErrorAction)
                .To<Mp4>(@"c:\source\ffmpeg\bin\temp\foo.mp4", SettingsCollection.ForOutput(new OverwriteOutput()));

            factory.Render(); 
        }

        public static void OnSuccessAction(ICommandFactory factory, ICommand command, ICommandProcessor results)
        {
            //results.StdOut
            // - contains the results ffmpeg command line 
            System.Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
            System.Console.WriteLine("Succcess");
            System.Console.WriteLine(results.StdOut);
            System.Console.ForegroundColor = System.ConsoleColor.White;
        }

        public static void OnErrorAction(ICommandFactory factory, ICommand command, ICommandProcessor results)
        {
            //results.StdOut
            // - contains the results ffmpeg command line 
            System.Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
            System.Console.WriteLine("Error");
            System.Console.WriteLine(results.StdOut);
            System.Console.ForegroundColor = System.ConsoleColor.White;
        }
    }
}
