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
    internal partial class Helpers
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

     

        public static double GetLength(Commandv2 command)
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
        public static double GetLength(Commandv2 command, Filterchainv2 filterchain)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var finalFilterLength = 0d;
            var calculatedFilterOutputDictionary = new Dictionary<string, CommandResourcev2>();
            var filterchainIndex = command.FiltergraphObject.FilterchainList.FindIndex(f => 
                filterchain.OutputList.Any(output => f.Output.Resource.Map == output.Resource.Map));


            command.FiltergraphObject.FilterchainList
                .GetRange(0, filterchainIndex + 1)
                .ForEach(filter =>
                {
                    var resourceList = new List<CommandResourcev2>();
                    var filterlistMaps = filter.ResourceList.Select(f => f.Map).ToList();
                    var commandOnlyResourcesFromReceiptsRaw = command.CommandOnlyResourcesFromReceipts(filter.ResourceList);
                    var commandOnlyResourcesFromReceipts = commandOnlyResourcesFromReceiptsRaw.Select(r =>
                    {
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
        [Obsolete("BreakReceipts is obsolete, use BreakReceipts with CommandReceipt resources.")]
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
        
        /// <summary>
        /// Breaks does command receipts into divisable subsets that then can be used to apply filters in chunks that ffmpeg will accept. while still abstracting from the user.
        /// </summary>
        public static List<CommandReceipt[]> BreakReceipts(int division, params CommandReceipt[] receipts)
        {
            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }

            var index = 0;
            var subDivision = division - 1;
            var breakouts = new List<CommandReceipt[]>();
            var resourcesRemainderCount = receipts.Length;
            resourcesRemainderCount -= (resourcesRemainderCount > division)
                                            ? division
                                            : receipts.Length;
            breakouts.Add(receipts.SubArray(0, division));
            while (resourcesRemainderCount > 0)
            {
                index++;
                var length = (resourcesRemainderCount > subDivision)
                                    ? subDivision
                                    : resourcesRemainderCount;
                resourcesRemainderCount -= length;
                breakouts.Add(receipts.SubArray(1 + (index * subDivision), length));
            }

            return breakouts;
        }

    }
}
