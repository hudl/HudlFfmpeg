using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class CommandConfigurationExtensions
    {
        public static CommandConfiguration Set(this CommandConfiguration configuration, CommandConfigurationFlagTypes flag, bool isOn)
        {
            var hasFlag = configuration.Has(flag);
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

        public static bool Has(this CommandConfiguration configuration, CommandConfigurationFlagTypes flag)
        {
            return (configuration.Flags & flag) == flag;
        }
    }

}
