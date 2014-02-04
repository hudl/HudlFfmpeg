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
                    { ScalePresetType.Sd480, new Point(852, 480) }, 
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
        public static double GetLength(CommandResourcev2 commandResource)
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
                    var lengthFromInputs = s.LengthFromInputs(new List<CommandResourcev2> { commandResource });
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
        public static double GetLength(List<CommandResourcev2> resourceList)
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
        public static double GetLength(Commandv2 command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (command.Objects.Filtergraph.FilterchainList.Count > 0)
            {
                return GetLength(command, command.Objects.Filtergraph.FilterchainList.Last());
            }
            else
            {
                return command.Objects.Inputs.Sum(r => GetLength(r));
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
            var filterchainIndex = command.Objects.Filtergraph.FilterchainList.FindIndex(f => f.Id == filterchain.Id);

            command.Objects.Filtergraph.FilterchainList
                .GetRange(0, filterchainIndex + 1)
                .ForEach(filter =>
                {
                    var resourceList = new List<CommandResourcev2>();
                    var filterlistMaps = filter.ReceiptList.Select(f => f.Map).ToList();
                    var commandOnlyResourcesFromReceiptsRaw = command.ResourcesFromReceipts(filter.ReceiptList.Where(r => r.Type == CommandReceiptType.Input).ToList());
                    var commandOnlyResourcesFromReceipts = commandOnlyResourcesFromReceiptsRaw.Select(r =>
                    {
                        var newLength = GetLength(r);
                        var newResource = r.Resource.Copy<IResource>();
                        newResource.Length = TimeSpan.FromSeconds(newLength);
                        var newCommandResourceTemp = CommandResourcev2.Create(newResource);
                        newCommandResourceTemp.Owner = r.Owner;
                        return newCommandResourceTemp;
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
                    filter.OutputList.ForEach(output =>
                        {
                            output.Length = filterLength.HasValue ? filterLength.Value : TimeSpan.FromSeconds(0);
                            var newCommandResource = CommandResourcev2.Create(output.Output());
                            newCommandResource.Owner = command;
                            calculatedFilterOutputDictionary.Add(output.Resource.Map, newCommandResource);
                        });
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
