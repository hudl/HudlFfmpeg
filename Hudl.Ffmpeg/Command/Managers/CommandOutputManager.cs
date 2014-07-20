using System;
using System.Collections.Generic;

namespace Hudl.Ffmpeg.Command.Managers
{
    /// <summary>
    /// A manager that controls the addition of new outputs to an Ffmpeg command.
    /// </summary>
    public class CommandOutputManager
    {
        private CommandOutputManager(FfmpegCommand owner)
        {
            Owner = owner;
        }    

        private FfmpegCommand Owner { get; set; }

        public void Add(CommandOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            output.Owner = Owner; 

            if (Owner.Objects.ContainsOutput(output))
            {
                throw new ArgumentException("Cannot add the same command output twice to command.", "output");
            }

            Owner.Objects.Outputs.Add(output);
        }

        public void AddRange(List<CommandOutput> outputList)
        {
            if (outputList == null || outputList.Count == 0)
            {
                throw new ArgumentException("Cannot add outputs from a list that is null or empty.", "outputList");
            }

            outputList.ForEach(Add);
        }

        internal static CommandOutputManager Create(FfmpegCommand owner)
        {
            return new CommandOutputManager(owner);
        }

    }
}
