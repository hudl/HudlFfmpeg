using System;
using System.Collections.Generic;
using System.Linq;

namespace Hudl.Ffmpeg.Command.Managers
{
    public class CommandResourceManager
    {
        private CommandResourceManager(FfmpegCommand owner)
        {
            Owner = owner;
        }

        public static CommandResourceManager Create(FfmpegCommand owner)
        {
            return new CommandResourceManager(owner);    
        }

        private FfmpegCommand Owner { get; set; }

        public CommandReceipt Add(CommandResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Owner = Owner;

            if (Owner.Objects.ContainsInput(resource.GetReceipt()))
            {
                throw new ArgumentException("Command already contains the specified resource.", "resource");
            }

            Owner.Objects.Inputs.Add(resource);

            return resource.GetReceipt();
        }

        public List<CommandReceipt> AddRange(List<CommandResource> resources)
        {
            if (resources == null || resources.Count == 0)
            {
                throw new ArgumentException("Cannot add resources from a list that is null or empty.", "resources");
            }

            return resources.Select(Add).ToList();
        }

        public CommandReceipt Insert(int index, CommandResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            
            resource.Owner = Owner;

            if (Owner.Objects.ContainsInput(resource.GetReceipt()))
            {
                throw new ArgumentException("Command already contains the specified resource.", "resource");
            }


            Owner.Objects.Inputs.Insert(index, resource);

            return resource.GetReceipt();
        }

        public CommandReceipt Replace(CommandReceipt replace, CommandResource replaceWith)
        {
            if (replace == null)
            {
                throw new ArgumentNullException("replace");
            }
            if (replaceWith == null)
            {
                throw new ArgumentNullException("replaceWith");
            }
            if (!Owner.Objects.ContainsInput(replace))
            {
                throw new ArgumentException("Command does not contain the resource to replace.", "replace");
            }

            replaceWith.Owner = Owner;
            
            if (Owner.Objects.ContainsInput(replaceWith.GetReceipt()))
            {
                throw new ArgumentException("Command already contains the specified resource.", "replaceWith");
            }

            var replaceIndex = Owner.Objects.Inputs.FindIndex(c => c.Resource.Map == replace.Map);
            Owner.Objects.Inputs.RemoveAt(replaceIndex);
            Owner.Objects.Inputs.Insert(replaceIndex, replaceWith);

            return replaceWith.GetReceipt();
        }
    }
}
