using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Resources;

namespace Hudl.Ffmpeg.Sugar
{
    public static class CommandOutputExtensions
    {
        public static void ValidateRecipts(CommandOutput output, CommandReceipt receipt)
        {
            if (output.Owner== null)
            {
                throw new ArgumentException("Output must contain an owner before sugar is allowed.", "output");
            }

            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }
        }

        public static CommandOutput MapVideoStream(this CommandOutput output, CommandReceipt receipt)
        {
            ValidateRecipts(output, receipt);

            var indexOfResource = output.Owner.Objects.Inputs.FindIndex(r => r.Resource.Map == receipt.Map);

            output.Settings.Add(indexOfResource < 0
                                      ? new Map(receipt)
                                      : new Map(string.Format("{0}:v", indexOfResource)));

            return output;
        }
        public static CommandOutput MapAudioStream(this CommandOutput output, CommandReceipt receipt)
        {
            ValidateRecipts(output, receipt);

            var indexOfResource = output.Owner.Objects.Inputs.FindIndex(r => r.Resource.Map == receipt.Map);
            
            output.Settings.Add(indexOfResource < 0
                                      ? new Map(receipt)
                                      : new Map(string.Format("{0}:a", indexOfResource)));

            return output;
        }
        public static List<CommandOutput> MapVideoStream(this List<CommandOutput> outputList, CommandReceipt receipt)
        {
            outputList.ForEach(output => output.MapVideoStream(receipt));

            return outputList;
        }
        public static List<CommandOutput> MapAudioStream(this List<CommandOutput> outputList, CommandReceipt receipt)
        {
            outputList.ForEach(output => output.MapAudioStream(receipt));

            return outputList;
        }
    }
}