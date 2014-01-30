using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hudl.Ffmpeg.Command.Managers
{
    public class Commandv2ResourceManager
    {
        private Commandv2ResourceManager(Commandv2 owner)
        {
            Owner = owner;
        }

        public static Commandv2ResourceManager Create(Commandv2 owner)
        {
            return new Commandv2ResourceManager(owner);    
        }

        private Commandv2 Owner { get; set; }

        public CommandReceipt Add(CommandResourcev2 resource)
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

        public List<CommandReceipt> AddRange(List<CommandResourcev2> resources)
        {
            if (resources == null || resources.Count == 0)
            {
                throw new ArgumentException("Cannot add resources from a list that is null or empty.", "resources");
            }

            return resources.Select(Add).ToList();
        }

        public CommandReceipt Insert(int index, CommandResourcev2 resource)
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

        public CommandReceipt Replace(CommandReceipt replace, CommandResourcev2 replaceWith)
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
