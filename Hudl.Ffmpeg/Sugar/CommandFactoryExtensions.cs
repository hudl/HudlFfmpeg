using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Sugar
{
    public static class CommandFactoryExtensions
    {
        public static FFmpegCommand CreateOutputCommand(this CommandFactory factory)
        {
            factory.AddCommandAsOutput(FFmpegCommand.Create(factory));

            return factory.CommandList[factory.CommandList.Count - 1];
        }
        public static FFmpegCommand CreateResourceCommand(this CommandFactory factory)
        {
            factory.AddCommandAsResource(FFmpegCommand.Create(factory));

            return factory.CommandList[factory.CommandList.Count - 1];
        }
    }
}
