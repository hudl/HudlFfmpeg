using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hudl.Ffmpeg.Command.Managers
{
    public class Commandv2OutputManager
    {
        private Commandv2OutputManager(Commandv2 owner)
        {
            Owner = owner;
        }    

        public static Commandv2OutputManager Create(Commandv2 owner)
        {
            return new Commandv2OutputManager(owner);
        }

        private Commandv2 Owner { get; set; }

        private void Add(CommandOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            output.Owner = Owner; 

            if (Owner.Objects.ContainsOutput(output.GetReceipt()))
            {
                throw new ArgumentException("Cannot add the same command output twice to command.", "output");
            }

            Owner.Objects.Outputs.Add(output);
        }
    }
}
