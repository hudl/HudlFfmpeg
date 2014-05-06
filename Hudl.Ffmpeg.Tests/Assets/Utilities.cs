using System.IO;

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
    }
}
