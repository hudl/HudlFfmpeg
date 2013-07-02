using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// helper class that helps with validation of objects in a ffmpeg project
    /// </summary>
    internal class Helpers
    {
        /// <summary>
        /// calculates the real time length based on the contents
        /// </summary>
        public static double GetLength(CommandResource<IResource> resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            
            return resource.Resource.Length.TotalSeconds + 
                resource.Settings.Items.Sum(s =>
                {
                    var lengthOverride = s.LengthOverride;
                    var lengthDifference = s.LengthDifference;
                    var lengthFromInputs = s.LengthFromInputs(new List<CommandResource<IResource>>
                        {
                            resource
                        });
                    if (lengthFromInputs.HasValue)
                    {
                        return lengthFromInputs.Value.TotalSeconds;
                    }
                    if (lengthOverride.HasValue)
                    {
                        return lengthOverride.Value.TotalSeconds;
                    }
                    if (lengthDifference.HasValue)
                    {
                        return lengthDifference.Value.TotalSeconds;
                    }
                    return 0D; 
                });
        }
        
        /// <summary>
        /// calculates the real time length based on the contents
        /// </summary>
        public static double GetLength(List<CommandResource<IResource>> resourceList)
        {
            if (resourceList == null)
            {
                throw new ArgumentNullException("resourceList");
            }
            return resourceList.Sum(cr => GetLength(cr)); 
        }
        
        /// <summary>
        /// calculates the real time length based on the contents
        /// </summary>
        public static double GetLength(Command<IResource> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var commandResourceLength = GetLength(command.ResourceList);
            var commandFiltergraphLength = command.Filtergraph.Filterchains.Sum(filterchain =>
                {
                    var resourceList = command.ResourcesFromReceipts(filterchain.ResourceList);
                    return filterchain.Filters.List.Sum(filter =>
                        {
                            var lengthOverride = filter.LengthOverride;
                            var lengthDifference = filter.LengthDifference;
                            var lengthFromInputs = filter.LengthFromInputs(resourceList);
                            if (lengthFromInputs.HasValue)
                            {
                                return lengthFromInputs.Value.TotalSeconds;
                            }
                            if (lengthOverride.HasValue)
                            {
                                return lengthOverride.Value.TotalSeconds;
                            }
                            if (lengthDifference.HasValue)
                            {
                                return lengthDifference.Value.TotalSeconds;
                            }
                            return 0D;
                        });
                });
            return commandResourceLength + commandFiltergraphLength; 
        }

        /// <summary>
        /// escapes the path of the provided resource.
        /// </summary>
        public static string EscapePath(IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return string.Format("\"{0}\"",
                                 resource.Path.Replace('\\', '/'));
        }
    }
}
