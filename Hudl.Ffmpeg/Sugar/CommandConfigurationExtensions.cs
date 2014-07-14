using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class CommandConfigurationExtensions
    {
        public static CommandConfiguration SetFlag(this CommandConfiguration configuration, CommandConfigurationFlagTypes flag, bool isOn)
        {
            var hasFlag = configuration.HasFlag(flag);
            if (isOn && !hasFlag)
            {
                configuration.Flags |= flag;
            }
            else if (!isOn && hasFlag)
            {
                configuration.Flags &= flag;
            }

            return configuration;
        }

        public static bool HasFlag(this CommandConfiguration configuration, CommandConfigurationFlagTypes flag)
        {
            return (configuration.Flags & flag) == flag;
        }
    }
}
