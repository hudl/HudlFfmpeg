using System.IO;
using System.Runtime.InteropServices;
using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Tests.Assets;

public static class Utilities
{
    private static string GetAssetsDirectory()
    {
        return (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            ? Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\samples\assets"))
            : Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../../Samples/Assets"));
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
        var outputPath = Path.GetTempPath();
        var ffmpegPath = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? "ffmpeg.exe" : "ffmpeg";
        var ffprobePath = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? "ffprobe.exe" : "ffprobe";

        ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, ffmpegPath, ffprobePath);
    }
}