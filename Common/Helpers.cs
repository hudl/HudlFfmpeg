using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// helper class that helps with validation of objects in a ffmpeg project
    /// </summary>
    internal class Helpers
    {
        public static string NewMap()
        {
            return string.Concat("mp", Guid.NewGuid().ToString().Substring(0, 8));
        }

        /// <summary>
        /// gets the path/domain from a full name base
        /// </summary>
        public static string GetPathFromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty", "fullName");
            }

            var fullNameNormalized = fullName.Replace("\\", "/");
            var fullNameFinalIndexOf = fullNameNormalized.LastIndexOf("/", System.StringComparison.Ordinal);
            return fullNameFinalIndexOf == -1 
                ? string.Empty 
                : fullNameNormalized.Substring(0, fullNameFinalIndexOf + 1);
        }

        /// <summary>
        /// gets the name from a full name base
        /// </summary>
        public static string GetNameFromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty", "fullName");
            }

            var fullNameNormalized = fullName.Replace("\\", "/");
            var fullNameFinalIndexOf = fullNameNormalized.LastIndexOf("/", System.StringComparison.Ordinal);
            return fullNameFinalIndexOf == -1 
                ? string.Empty 
                : fullNameNormalized.Substring(fullNameFinalIndexOf + 1);
        }

        /// <summary>
        /// calculates the real time length based on the contents
        /// </summary>
        public static double GetLength(CommandResource<IResource> commandResource)
        {
            if (commandResource == null)
            {
                throw new ArgumentNullException("commandResource");
            }

            var resourceDefaultLength = commandResource.Resource.Length.TotalSeconds;
            var resourceSettingsLength = 0d; 
            if (commandResource.Settings.Count > 0)
            {
                resourceSettingsLength = commandResource.Settings.Items.Min(s =>
                {
                    var lengthFromInputs = s.LengthFromInputs(new List<CommandResource<IResource>> { commandResource });
                    return lengthFromInputs.HasValue ? lengthFromInputs.Value.TotalSeconds : 0D;
                });
            }
            return resourceSettingsLength > 0d
                       ? resourceSettingsLength
                       : resourceDefaultLength;
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
                            var lengthFromInputs = filter.LengthFromInputs(resourceList);
                            return lengthFromInputs.HasValue ? lengthFromInputs.Value.TotalSeconds : 0D;
                        });
                });
            return commandResourceLength + commandFiltergraphLength; 
        }

        /// <summary>
        /// calculates the real time length based on the contents
        /// </summary>
        public static double GetLength(Command<IResource> command, Filterchain<IResource> filterchain)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (filterchain == null)
            {
                throw new ArgumentNullException("command");
            }

            var filterchainInputMaps = filterchain.ResourceList.Select(r => r.Map).ToList();
            var filterchainIndexInCommand = command.Filtergraph.FilterchainList.FindIndex(f => f.Output.Resource.Map == filterchain.Output.Resource.Map);
            var filterchainsInSequenceTo = command.Filtergraph.FilterchainList
                                                  .GetRange(0, filterchainIndexInCommand + 1)
                                                  .Where(f => filterchainInputMaps.Contains(f.Output.Resource.Map))
                                                  .ToList();
            var commandOnlyResourcesFromReceipts = command.CommandOnlyResourcesFromReceipts(filterchain.ResourceList);
            var filterchainOutputsFromSequence = filterchainsInSequenceTo.Select(f => new CommandResource<IResource>(command, f.GetOutput(command).GetOutput())).ToList();
            var commandResourceLength = GetLength(commandOnlyResourcesFromReceipts);
            var commandFiltergraphLength = filterchain.Filters.List.Sum(filter =>
                {
                    var lengthFromInputs = filter.LengthFromInputs(filterchainOutputsFromSequence);
                    return lengthFromInputs.HasValue ? lengthFromInputs.Value.TotalSeconds : 0D;
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
                                 resource.FullName.Replace('\\', '/'));
        }

        public static List<CommandResourceReceipt[]> BreakReceipts(int division, params CommandResourceReceipt[] resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }

            var index = 0;
            var subDivision = division - 1;
            var breakouts = new List<CommandResourceReceipt[]>();
            var resourcesRemainderCount = resources.Length;
            resourcesRemainderCount -= (resourcesRemainderCount > division)
                                            ? division
                                            : resources.Length;
            breakouts.Add(resources.SubArray(0, division));
            while (resourcesRemainderCount > 0)
            {
                index++;
                var length = (resourcesRemainderCount > subDivision)
                                    ? subDivision
                                    : resourcesRemainderCount;
                resourcesRemainderCount -= length;
                breakouts.Add(resources.SubArray(1 + (index * subDivision), length));
            }

            return breakouts;
        }

    }
}
