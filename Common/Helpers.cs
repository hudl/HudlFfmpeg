using System;
using System.Drawing;
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
        /// <summary>
        /// returns a list of scaling presets for ffmpeg.
        /// </summary>
        public static Dictionary<ScalePresetType, Point> ScalingPresets
        {
            get
            {
                return new Dictionary<ScalePresetType, Point>
                {
                    { ScalePresetType.Svga, new Point(800, 600) }, 
                    { ScalePresetType.Xga, new Point(1024, 768) }, 
                    { ScalePresetType.Ega, new Point(640, 350) }, 
                    { ScalePresetType.Sd240, new Point(432, 240) }, 
                    { ScalePresetType.Sd360, new Point(640, 360) }, 
                    { ScalePresetType.Hd480, new Point(854, 480) }, 
                    { ScalePresetType.Hd720, new Point(1280, 720) },
                    { ScalePresetType.Hd1080, new Point(1920, 1080) }
                };
            }
        } 

        /// <summary>
        /// generates a new resource map variable
        /// </summary>
        /// <returns></returns>
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
        /// gets the extension from a full name base
        /// </summary>
        public static string GetExtensionFromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty", "fullName");
            }

            var queryStringIndex = fullName.IndexOf("?", StringComparison.Ordinal);
            if (queryStringIndex != -1)
            {
                fullName = fullName.Substring(0, queryStringIndex);
            }

            var finalIndex = fullName.LastIndexOf(".", StringComparison.Ordinal);
            if (finalIndex != -1)
            {
                fullName = fullName.Substring(finalIndex + 1);
            }

            return fullName;
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

            if (command.Filtergraph.FilterchainList.Count > 0)
            {
                return GetLength(command, command.Filtergraph.FilterchainList.Last());
            }
            else
            {
                return command.ResourceList.Sum(r => GetLength(r));
            }
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

            var finalFilterLength = 0d;
            var calculatedFilterOutputDictionary = new Dictionary<string, CommandResource<IResource>>();
            var calculatedPrepOutputDictionary = new Dictionary<string, CommandResource<IResource>>();
            var filterchainIndex = command.Filtergraph.FilterchainList.FindIndex(f => f.Output.Resource.Map == filterchain.Output.Resource.Map);

            if (command.CommandList.Count > 0)
            {
                command.CommandList.ForEach(c =>
                    {
                        var commandOutputLength = GetLength(c);
                        var newResource = c.Output.Resource.Copy<IResource>();
                        newResource.Length = TimeSpan.FromSeconds(commandOutputLength);
                        var newCommandResource = new CommandResource<IResource>(c, newResource);
                        calculatedPrepOutputDictionary.Add(c.Output.Resource.Map, newCommandResource);
                    });    
            }

            command.Filtergraph.FilterchainList
                .GetRange(0, filterchainIndex + 1)
                .ForEach(filter =>
                {
                    var resourceList = new List<CommandResource<IResource>>();
                    var filterlistMaps = filter.ResourceList.Select(f => f.Map).ToList();
                    var commandOnlyResourcesFromReceiptsRaw = command.CommandOnlyResourcesFromReceipts(filter.ResourceList);
                    var commandOnlyResourcesFromReceipts = commandOnlyResourcesFromReceiptsRaw.Select(r =>
                        {
                            if (calculatedPrepOutputDictionary.ContainsKey(r.Resource.Map))
                            {
                                return calculatedPrepOutputDictionary[r.Resource.Map];
                            }
                            var newLength = GetLength(r);
                            var newResource = r.Resource.Copy<IResource>();
                            newResource.Length = TimeSpan.FromSeconds(newLength);
                            return new CommandResource<IResource>(r.Parent, newResource);
                        }).ToList();
                    var filterOnlyResourceFromReceipts = calculatedFilterOutputDictionary.Where(r => filterlistMaps.Contains(r.Key))
                                                                                         .Select(r => r.Value).ToList();

                    if (commandOnlyResourcesFromReceipts.Count > 0)
                    {
                        resourceList.AddRange(commandOnlyResourcesFromReceipts);
                    }
                    if (filterOnlyResourceFromReceipts.Count > 0)
                    {
                        resourceList.AddRange(filterOnlyResourceFromReceipts);
                    }
                   
                    var filterLength = filter.Filters.Items.First().LengthFromInputs(resourceList);
                    finalFilterLength = filterLength.HasValue ? filterLength.Value.TotalSeconds : 0d;
                    filter.Output.Length = filterLength.HasValue ? filterLength.Value : TimeSpan.FromSeconds(0);
                    var newCommandResource = new CommandResource<IResource>(command, filter.Output.GetOutput());
                    calculatedFilterOutputDictionary.Add(filter.Output.Resource.Map, newCommandResource);
                });


            return finalFilterLength;
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

        /// <summary>
        /// Breaks does command receipts into divisable subsets that then can be used to apply filters in chunks that ffmpeg will accept. while still abstracting from the user.
        /// </summary>
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
