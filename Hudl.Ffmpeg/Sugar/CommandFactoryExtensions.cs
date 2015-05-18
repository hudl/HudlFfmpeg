using Hudl.FFmpeg.Command;

namespace Hudl.FFmpeg.Sugar
{
    public static class CommandFactoryExtensions
    {
        public static FFmpegCommand CreateOutputCommand(this CommandFactory factory)
        {
            factory.AddCommandAsOutput(FFmpegCommand.Create(factory));

            //TODO: fix
            return (FFmpegCommand)factory.CommandList[factory.CommandList.Count - 1];
        }
        public static FFmpegCommand CreateResourceCommand(this CommandFactory factory)
        {
            factory.AddCommandAsResource(FFmpegCommand.Create(factory));

            //TODO: fix
            return (FFmpegCommand)factory.CommandList[factory.CommandList.Count - 1];
        }
    }
}
