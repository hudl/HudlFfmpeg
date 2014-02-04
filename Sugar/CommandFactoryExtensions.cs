using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Sugar
{
    public static class CommandFactoryExtensions
    {
        public static FfmpegCommand AsOutput(this CommandFactory factory)
        {
            factory.AddToOutput(FfmpegCommand.Create(factory));
            return factory.CommandList[factory.CommandList.Count - 1];
        }
        public static FfmpegCommand AsResource(this CommandFactory factory)
        {
            factory.AddToResources(FfmpegCommand.Create(factory));
            return factory.CommandList[factory.CommandList.Count - 1];
        }
    }
}
