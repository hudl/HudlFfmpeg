using Hudl.FFmpeg.Samples.FFmpeg.Concat;
using Hudl.FFmpeg.Samples.FFmpeg.Convert;
using Hudl.FFmpeg.Samples.FFmpeg.Events;
using Hudl.FFmpeg.Samples.FFmpeg.Mapping;
using Hudl.FFmpeg.Samples.Metadata;
using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Filters.Templates;

namespace Hudl.FFmpeg.Samples
{
    class Program
    {
        private class SampleRun
        {
            public string Name { get; set; }
            public Action Action { get; set; }
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the Hudl.FFmpeg samples application.");
            Console.WriteLine("  please select from the following list of options");

            var options = new Dictionary<string, SampleRun>
            {
                { "1", new SampleRun { Name = "ConcatAudio", Action = ConcatAudio.Run} },
                { "2", new SampleRun { Name = "ConcatVideo", Action = ConcatVideo.Run} },
                { "3", new SampleRun { Name = "ConcatVideoAndAudio", Action = ConcatVideoAndAudio.Run} },
                { "4", new SampleRun { Name = "DissolveVideo", Action = DissolveVideo.Run} },
                { "5", new SampleRun { Name = "AngleSwipeVideo", Action = AngleSwipeVideo.Run} },
                { "6", new SampleRun { Name = "WhiteFlashVideo", Action = CameraFlashVideo.Run} },
                { "7", new SampleRun { Name = "MapMultipleStreamsToMultipleOutputs", Action = MapMultipleStreamsToMultipleOutputs.Run} },
                { "8", new SampleRun { Name = "MapMultipleStreamsToSingleOutput", Action = MapMultipleStreamsToSingleOutput.Run} },
                { "9", new SampleRun { Name = "ExtractFrame", Action = ExtractFrame.Run} },
                { "10", new SampleRun { Name = "AttachOnSuccessOnErrorEvents", Action = AttachOnSuccessOnErrorEvents.Run} },
                { "11", new SampleRun { Name = "MetadataCalculation", Action = MetadataCalculation.Run} },
            };

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            foreach (var option in options)
            {
                Console.WriteLine(string.Format("  [{0}] - {1}", option.Key, option.Value.Name));
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();
            var isRunning = true;
            while (isRunning)
            {
                Console.Write("> ");

                var input = Console.ReadLine();

                if (input.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    isRunning = false;
                    continue;
                }

                if (!options.ContainsKey(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option, choose again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                options[input].Action();
            }
        }
    }
}
