using Hudl.Ffmpeg.Command;

namespace Hudl.Ffmpeg.Sugar
{
    public static class CommandFactoryExtensions
    {
        public static FfmpegCommand CreateOutputCommand(this CommandFactory factory)
        {
            factory.AddCommandAsOutput(FfmpegCommand.Create(factory));

            return factory.CommandList[factory.CommandList.Count - 1];
        }
        public static FfmpegCommand CreateResourceCommand(this CommandFactory factory)
        {
            factory.AddCommandAsResource(FfmpegCommand.Create(factory));

            return factory.CommandList[factory.CommandList.Count - 1];
        }
    }
}
