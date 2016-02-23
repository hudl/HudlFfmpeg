using System.IO;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Samples.Assets
{
    public class Utilities
    {
        public static string GetAssetsDirectory()
        {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\samples\assets"));
        }
        public static string GetAudioFile()
        {
            return Path.Combine(GetAssetsDirectory(), "sample-audio.m4a");
        }
        public static string GetVideoFile()
        {
            return Path.Combine(GetAssetsDirectory(), "sample-video.mp4");
        }
        public static string GetImageFile()
        {
            return Path.Combine(GetAssetsDirectory(), "sample-image.jpg");
        }

        public static void SetGlobalAssets()
        {
            const string outputPath = "c:/source/ffmpeg/bin/temp";
            const string ffmpegPath = "c:/source/ffmpeg/bin/ffmpeg.exe";
            const string ffprobePath = "c:/source/ffmpeg/bin/FFprobe.exe";

            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, ffmpegPath, ffprobePath);
        }
    }
}
