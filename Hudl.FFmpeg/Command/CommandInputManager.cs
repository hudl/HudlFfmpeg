using System;
using System.Collections.Generic;
using System.Linq;

namespace Hudl.FFmpeg.Command
{
    /// <summary>
    /// A manager that will manage the resource for an ffmpeg command
    /// </summary>
    public class CommandInputManager
    {
        private CommandInputManager(FFmpegCommand owner)
        {
            Owner = owner;
        }

        private FFmpegCommand Owner { get; set; }

        public List<StreamIdentifier> Add(CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Owner = Owner;

            if (Owner.Objects.ContainsInput(resource))
            {
                throw new ArgumentException("Command already contains the specified resource.", "resource");
            }

            Owner.Objects.Inputs.Add(resource);

            return resource.GetStreamIdentifiers();
        }

        public List<StreamIdentifier> AddRange(List<CommandInput> resources)
        {
            if (resources == null || resources.Count == 0)
            {
                throw new ArgumentException("Cannot add resources from a list that is null or empty.", "resources");
            }

            return resources.SelectMany(Add).ToList();
        }

        public List<StreamIdentifier> Insert(int index, CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            
            resource.Owner = Owner;

            if (Owner.Objects.ContainsInput(resource))
            {
                throw new ArgumentException("Command already contains the specified resource.", "resource");
            }


            Owner.Objects.Inputs.Insert(index, resource);

            return resource.GetStreamIdentifiers();
        }

        internal static CommandInputManager Create(FFmpegCommand owner)
        {
            return new CommandInputManager(owner);
        }
    }
}
