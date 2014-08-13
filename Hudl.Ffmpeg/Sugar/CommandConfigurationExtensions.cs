using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;

namespace Hudl.FFmpeg.Sugar
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
