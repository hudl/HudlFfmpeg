using System;
using System.Collections.Generic;

namespace Hudl.FFmpeg.Command
{
    /// <summary>
    /// A manager that controls the addition of new outputs to an FFmpeg command.
    /// </summary>
    public class CommandOutputManager
    {
        private CommandOutputManager(FFmpegCommand owner)
        {
            Owner = owner;
        }    

        private FFmpegCommand Owner { get; set; }

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

        internal static CommandOutputManager Create(FFmpegCommand owner)
        {
            return new CommandOutputManager(owner);
        }

    }
}
