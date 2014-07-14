using System.IO;
using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Tests.Assets
{
    public class Utilities
    {
        public static string GetAssetsDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "assets");
        }
        public static string GetAudioFile()
        {
            return Path.Combine(GetAssetsDirectory(), "audio.mp3");
        }
        public static string GetVideoFile()
        {
            return Path.Combine(GetAssetsDirectory(), "video.mp4");
        }
        public static string GetImageFile()
        {
            return Path.Combine(GetAssetsDirectory(), "image.png");
        }

        public static void SetGlobalAssets()
        {
            const string outputPath = "c:/source/ffmpeg/bin/temp";
            const string ffmpegPath = "c:/source/ffmpeg/bin/ffmpeg.exe";
            const string ffprobePath = "c:/source/ffmpeg/bin/ffprobe.exe";

            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, ffmpegPath, ffprobePath, outputPath);
        }

    }
}
