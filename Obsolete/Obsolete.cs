using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Settings.Obsolete;
using Hudl.Ffmpeg.Settings.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Templates.Obsolete;
using Hudl.Ffmpeg.Templates.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Filters.Obsolete;
using Hudl.Ffmpeg.Filters.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Command.Obsolete;
using log4net;

/* 
* Stage 1 Obsolete is a non-error state for objects that have full tested and working solutions. 
* The lifespan of an object in stage 1 obsoletion is 2 months.
*/
#region Stage 1 Obsolete
namespace Hudl.Ffmpeg.Common
{
    internal partial class Helpers
    {
        [Obsolete("GetLength is obsolete, use GetLength with Commandv2 resources.", false)]
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

        [Obsolete("GetLength is obsolete, use GetLength with Commandv2 resources.", false)]
        public static double GetLength(List<CommandResource<IResource>> resourceList)
        {
            if (resourceList == null)
            {
                throw new ArgumentNullException("resourceList");
            }
            return resourceList.Sum(cr => GetLength(cr));
        }

        [Obsolete("GetLength is obsolete, use GetLength with Commandv2 resources.", false)]
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

        [Obsolete("GetLength is obsolete, use GetLength with Commandv2 resources.", false)]
        public static double GetLength(Command<IResource> command, Filterchain<IResource> filterchain)
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

        [Obsolete("BreakReceipts is obsolete, use BreakReceipts with CommandReceipt resources.", false)]
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

namespace Hudl.Ffmpeg.Common.Obsolete
{
    public class Validate
    {
        [Obsolete("IsSettingFor<TSetting> is obsolete, do not reference Obsolete namespace.", false)]
        public static bool IsSettingFor<TSetting>(TSetting item, SettingsCollectionResourceType type)
            where TSetting : ISetting
        {
            var itemType = item.GetType();
            var matchingAttribute = Common.Validate.GetAttribute<Settings.BaseTypes.SettingsApplicationAttribute>(itemType);

            return matchingAttribute != null && type == matchingAttribute.ResourceType;
        }

        [Obsolete("GetSettingData<TSetting> is obsolete, do not reference Obsolete namespace.", false)]
        internal static Settings.BaseTypes.SettingsApplicationData GetSettingData<TSetting>()
            where TSetting : ISetting
        {
            return Common.Validate.GetSettingData(typeof(TSetting));
        }

        [Obsolete("GetSettingData<TSetting> is obsolete, do not reference Obsolete namespace.", false)]
        internal static Settings.BaseTypes.SettingsApplicationData GetSettingData<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            return Common.Validate.GetSettingData(setting.GetType());
        }

        [Obsolete("GetSettingCollectionData is obsolete, do not reference Obsolete namespace.", false)]
        internal static Dictionary<Type, Settings.BaseTypes.SettingsApplicationData> GetSettingCollectionData(SettingsCollection collection)
        {
            var settingsDictionary = new Dictionary<Type, Settings.BaseTypes.SettingsApplicationData>();
            collection.SettingsList.ForEach(setting =>
            {
                var settingsType = setting.GetType();
                if (settingsDictionary.ContainsKey(settingsType)) return;
                settingsDictionary.Add(settingsType, GetSettingData(setting));
            });
            return settingsDictionary;
        }
    }
}

namespace Hudl.Ffmpeg.Filters.Obsolete.BaseTypes
{
    [Obsolete("Filterchain using Filterchain<TResource> is obsolete, use Filterchainv2 reference.", false)]
    public class Filterchain
    {
        /// <summary>
        /// Returns a new instance of the filterchain
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public static Filterchain<TResource> FilterTo<TResource>(params IFilter[] filters)
            where TResource : IResource, new()
        {
            return FilterTo(new TResource(), filters);
        }

        /// <summary>
        /// Returns a new instance of the filterchain
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public static Filterchain<TResource> FilterTo<TResource>(TResource output, params IFilter[] filters)
            where TResource : IResource
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            return new Filterchain<TResource>(output, filters);
        }
       
    }

    [Obsolete("IFilter using Command<TResource> is obsolete, use Commandv2 reference.", false)]
    public interface IFilter
    {
        /// <summary>
        /// the command name for the affect
        /// </summary>
        string Type { get; }

        /// <summary>
        /// maximum number of inputs that the filter can support
        /// </summary>
        int MaxInputs { get; }

        /// <summary>
        /// the length override function, overrided when a fitler requires a length change of output calculated from the resources.
        /// </summary>
        /// <returns>Null indicates that the length difference does not apply</returns>
        TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources);

        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string ToString();

        /// <summary>
        /// sets up the filter based on the settings in the filterchain
        /// </summary>
        void Setup(Command<IResource> command, Filterchain<IResource> filterchain);
    }

    [Obsolete("IFilterProcessor using Command<TResource> is obsolete, use Commandv2 reference.", false)]
    public interface IFilterProcessor
    {
        /// <summary>
        /// Generates a list of Single Input, Single Output prepatory commands, this restraint is enforeced.
        /// </summary>
        /// <typeparam name="TOutput">The command output type</typeparam>
        /// <param name="command">The Command parent that the filter is by association a part of</param>
        /// <param name="filterchain">The Filterchain parent that the filter is part of</param>
        void PrepCommands<TOutput, TResource>(Command<TOutput> command, Filterchain<TResource> filterchain)
            where TOutput : IResource
            where TResource : IResource, new();
    }

    [Obsolete("IFilterValidator using Command<TResource> is obsolete, use Commandv2 reference.", false)]
    interface IFilterValidator
    {
        /// <summary>
        /// Validates the Filter based on the command and filterchain logic
        /// </summary>
        /// <param name="command">The command that contains the Filterchain that holds the Filter</param>
        /// <param name="filterchain">The Filterchain that hold the Filter</param>
        /// <param name="resources">The Resource Receipts from the Command that the filter is to be applied against.</param>
        bool Validate(Command<IResource> command, Filterchain<IResource> filterchain, List<CommandResourceReceipt> resources);
    }

    [Obsolete("BaseFilter using Command<TResource> is obsolete, use Commandv2 reference.", false)]
    public abstract class BaseFilter : IFilter
    {
        protected BaseFilter(string type, int maxInputs)
        {
            Type = type;
            MaxInputs = maxInputs;
            CommandResources = new List<CommandResource<IResource>>();
        }

        /// <summary>
        /// Defines the filter type, name that is given to ffmpeg
        /// </summary>
        public string Type { get; protected set; }

        /// <summary>
        /// Defines the maximum number of allowable inputs to the filter
        /// </summary>
        public int MaxInputs { get; protected set; }

      

        /// <summary>
        /// Available at [Render] time, brings the resources as available objects to the filters
        /// </summary>
        protected List<CommandResource<IResource>> CommandResources { get; set; }

        /// <summary>
        /// Method, called during [Render] to bring forward all the necessary resources, necessary action for maximum abstraction from the user.
        /// </summary>
        /// <param name="command">The command chain the current filter belongs to.</param>
        /// <param name="filterchain">The filterchain that the filter belongs to</param>
        public void Setup(Command<IResource> command, Filterchain<IResource> filterchain)
        {
            CommandResources = command.ResourcesFromReceipts(new List<CommandResourceReceipt>(filterchain.Resources));

            if (CommandResources.Count == 0)
            {
                throw new InvalidOperationException("Cannot setup filter with a resource count of zero.");
            }
            if (CommandResources.Count > MaxInputs)
            {
                throw new InvalidOperationException("The filter has exceeded the maximum allowed number of inputs.");
            }
        }

        /// <summary>
        /// Quick way to calculate the output length after a filter has been applied.
        /// </summary>
        public virtual TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            var totalSeconds = resources.Sum(r => r.Resource.Length.TotalSeconds);
            return totalSeconds > 0d
                       ? (TimeSpan?)TimeSpan.FromSeconds(totalSeconds)
                       : null;
        }
    }

    [Obsolete("Filterchain<TOutput> is obsolete, use Filterchainv2 reference.", false)]
    public class Filterchain<TOutput>
        where TOutput : IResource
    {
        internal Filterchain(TOutput outputToUse)
        {
            if (outputToUse == null)
            {
                throw new ArgumentNullException("outputToUse");
            }

            ResourceList = new List<CommandResourceReceipt>();
            Output = new FilterchainOutput<TOutput>(this, outputToUse);
            Filters = new AppliesToCollection<IFilter>(outputToUse.GetType());
        }
        internal Filterchain(TOutput outputToUse, params IFilter[] filters)
            : this(outputToUse)
        {
            if (filters.Length > 0)
            {
                Filters.AddRange(filters);
            }
        }

        public static implicit operator Filterchain<IResource>(Filterchain<TOutput> filterchain)
        {
            var filterchainNew = new Filterchain<IResource>(filterchain.Output.Resource, filterchain.Filters.List.ToArray());
            if (filterchain.ResourceList.Count > 0)
            {
                filterchainNew.SetResources(filterchain.ResourceList);
            }
            return filterchainNew;
        }

        public AppliesToCollection<IFilter> Filters { get; protected set; }

        public ReadOnlyCollection<CommandResourceReceipt> Resources { get { return ResourceList.AsReadOnly(); } }

        public void SetResources(params CommandResourceReceipt[] resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Length == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            SetResources(new List<CommandResourceReceipt>(resources));
        }

        public void SetResources(List<CommandResourceReceipt> resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one resource.");
            }

            ResourceList = resources;
        }

        public Filterchain<TResource> Copy<TResource>()
            where TResource : IResource
        {
            return Filterchain.FilterTo(Output.Resource.Copy<TResource>(), Filters.List.ToArray());
        }

        public FilterchainOutput<TOutput> GetOutput(Command<IResource> command)
        {
            Output.Length = TimeSpan.FromSeconds(Helpers.GetLength(command, this));
            return Output;
        }

        #region Internals
        internal List<CommandResourceReceipt> ResourceList { get; set; }
        internal FilterchainOutput<TOutput> Output { get; set; }
        #endregion
    }

    [Obsolete("FilterchainOutput<TOutput> is obsolete, use FilterchainOutputv2 reference.", false)]
    public class FilterchainOutput<TResource>
       where TResource : IResource
    {
        internal FilterchainOutput(Filterchain<TResource> parent, TResource resource)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            Parent = parent;
            Resource = resource;
        }

        public TimeSpan Length { get; set; }

        public Filterchain<TResource> Parent { get; protected set; }

        public TResource GetOutput()
        {
            Resource.Length = Length;
            return Resource;
        }

        #region Internals
        internal TResource Resource { get; set; }
        #endregion
    }

    [Obsolete("Filtergraph> is obsolete, use Filtergraphv2 reference.", false)]
    public class Filtergraph
    {
        public Filtergraph()
        {
            FilterchainList = new List<Filterchain<IResource>>();
        }

        public ReadOnlyCollection<Filterchain<IResource>> Filterchains
        {
            get
            {
                return FilterchainList.AsReadOnly();
            }
        }

        public int Count { get { return FilterchainList.Count; } }

        public bool Contains<TOutput>(Filterchain<TOutput> filterchain)
            where TOutput : IResource
        {
            return FilterchainList.Any(f => f.Output.Resource.Map == filterchain.Output.Resource.Map);
        }

        public int IndexOf<TOutput>(Filterchain<TOutput> filterchain)
            where TOutput : IResource
        {
            return FilterchainList.FindIndex(f => f.Output.Resource.Map == filterchain.Output.Resource.Map);
        }

        /// <summary>
        /// Adds a new instance of a filterchain to the filtergraph
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public Filtergraph FilterTo<TResource>(params IFilter[] filters)
            where TResource : IResource, new()
        {
            var filterchain = Filterchain.FilterTo<TResource>(filters);

            return Add(filterchain);
        }

        /// <summary>
        /// Adds a new instance of a filterchain to the filtergraph
        /// </summary>
        /// <typeparam name="TResource">the Type of output for the new filterchain</typeparam>
        public Filtergraph FilterTo<TResource>(TResource output, params IFilter[] filters)
            where TResource : IResource
        {
            var filterchain = Filterchain.FilterTo<TResource>(output, filters);

            return Add(filterchain);
        }

        /// <summary>
        /// adds the given Filterchain to the Filtergraph
        /// </summary>
        /// <typeparam name="TOutput">the generic type of the filterchain</typeparam>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Add<TOutput>(Filterchain<TOutput> filterchain)
            where TOutput : IResource
        {
            FilterchainList.Add(filterchain);
            return this;
        }

        /// <summary>
        /// merges the given Filterchain to the Filtergraph
        /// </summary>
        /// <typeparam name="TOutput">the generic type of the filterchain</typeparam>
        /// <param name="filterchain">the filterchain to be added to the filtergraph</param>
        public Filtergraph Merge<TOutput>(Filterchain<TOutput> filterchain, FfmpegMergeOptionType optionType)
            where TOutput : IResource
        {
            var indexOfItem = IndexOf(filterchain);
            if (indexOfItem != -1 && optionType == FfmpegMergeOptionType.NewWins)
            {
                FilterchainList.RemoveAt(indexOfItem);
                FilterchainList.Insert(indexOfItem, filterchain);
            }
            else if (indexOfItem == -1)
            {
                FilterchainList.Add(filterchain);
            }

            return this;
        }

        /// <summary>
        /// removes the Filterchain at the given index from the Filtergraph
        /// </summary>
        /// <param name="index">the index of the desired Filterchain to be removed from the Filtergraph</param>
        public Filtergraph RemoveAt(int index)
        {
            FilterchainList.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// removes all the Filterchain matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public Filtergraph RemoveAll(Predicate<Filterchain<IResource>> pred)
        {
            FilterchainList.RemoveAll(pred);
            return this;
        }

        public override string ToString()
        {
            //perform simple validation on filter graph
            if (FilterchainList.Count == 0)
            {
                throw new InvalidOperationException("Filtergraph must contain at least one Filterchain.");
            }

            var filtergraph = new StringBuilder(100);
            FilterchainList.ForEach(filterchain =>
            {
                if (filtergraph.Length > 0) filtergraph.Append(";");
                filtergraph.Append(filterchain.ToString());
            });

            //return the formatted filter command string 
            return filtergraph.ToString();
        }

        #region Internals
        internal List<Filterchain<IResource>> FilterchainList { get; set; }
        #endregion
    }
}

namespace Hudl.Ffmpeg.Filters.Obsolete.Templates
{
    [Obsolete("Crossfade using Command<TResource> is obsolete, use Commandv2 reference.", false)]
    public class Crossfade : Blend, IFilterProcessor
    {
        private const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";
        private readonly SettingsCollection _outputSettings = SettingsCollection.ForOutput(
            new OverwriteOutput(),
            new VCodec(VideoCodecType.Copy));

        public Crossfade(TimeSpan duration, Filterchain<IResource> resolutionFilterchain)
        {
            Duration = duration;
            Option = BlendVideoOptionType.all_expr;
            ResolutionFilterchain = resolutionFilterchain;
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _duration = value;
                Expression = string.Format(CrossfadeAlgorithm, value.TotalSeconds);
            }
        }

        private Filterchain<IResource> ResolutionFilterchain { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return Duration;
        }

        public void PrepCommands<TOutput, TResource>(Command<TOutput> command, Filterchain<TResource> filterchain)
            where TOutput : IResource
            where TResource : IResource, new()
        {
            //verify that we have a resolution filterchain 
            if (ResolutionFilterchain == null)
            {
                throw new InvalidOperationException("A resolution is required for the cross fade command, because of the Blend filter.");
            }

            double resourceToLength;
            double resourceFromLength;
            Filterchain<IResource> filterchainCutupTransitionTo = null;
            Filterchain<IResource> filterchainCutupTransitionFrom = null;

            var resourceTo = command.CommandOnlyResourcesFromReceipts(filterchain.Resources[1]).FirstOrDefault();
            var resourceFrom = command.CommandOnlyResourcesFromReceipts(filterchain.Resources[0]).FirstOrDefault();
            var filterchainCutupBodyTo = command.FilterchainFromReceipt(filterchain.Resources[1]);
            var filterchainCutupBodyFrom = command.FilterchainFromReceipt(filterchain.Resources[0]);

            //validate that the resources in fact come from the appropriate command
            if (resourceTo == null && filterchainCutupBodyTo == null)
            {
                throw new InvalidOperationException("To resource does not belong to the Command or Command Factory.");
            }
            if (resourceFrom == null && filterchainCutupBodyFrom == null)
            {
                throw new InvalidOperationException("From resource does not belong to the Command or Command Factory.");
            }

            //get the filterchain body cutup
            if (filterchainCutupBodyTo != null)
            {
                resourceToLength = filterchainCutupBodyTo.GetOutput(command).Length.TotalSeconds;
                var hasPreExistingTrim = filterchainCutupBodyTo.Filters.Contains<Trim>();
                if (hasPreExistingTrim)
                {
                    var trimFilter = filterchainCutupBodyTo.Filters.Get<Trim>();
                    if (trimFilter.TimebaseUnit == VideoUnitType.Frames)
                    {
                        filterchainCutupBodyTo = null;
                    }
                }
                else
                {
                    filterchainCutupBodyTo = null;
                }
            }
            else
            {
                resourceToLength = resourceTo.Resource.Length.TotalSeconds;
            }

            if (filterchainCutupBodyFrom != null)
            {
                resourceFromLength = filterchainCutupBodyFrom.GetOutput(command).Length.TotalSeconds;
                var hasPreExistingTrim = filterchainCutupBodyFrom.Filters.Contains<Trim>();
                if (hasPreExistingTrim)
                {
                    var trimFilter = filterchainCutupBodyFrom.Filters.Get<Trim>();
                    if (trimFilter.TimebaseUnit == VideoUnitType.Frames)
                    {
                        filterchainCutupBodyFrom = null;
                    }
                }
                else
                {
                    filterchainCutupBodyFrom = null;
                }
            }
            else
            {
                resourceFromLength = resourceFrom.Resource.Length.TotalSeconds;
            }

            //factor the common lengths 
            var durationLength = Duration.TotalSeconds;
            var durationFromEndLength = resourceFromLength - durationLength;

            //an existing trim cannot be located, so a new one is required
            if (filterchainCutupBodyTo == null)
            {
                filterchainCutupBodyTo = new VideoCutTo<TResource>(durationLength, resourceToLength);
            }
            else
            {
                var trimFilter = filterchainCutupBodyTo.Filters.Get<Trim>();
                trimFilter.End -= Duration.TotalSeconds;
                trimFilter.Duration = trimFilter.End - trimFilter.Start;
                filterchainCutupBodyTo.Filters.Merge(trimFilter, FfmpegMergeOptionType.NewWins);
            }
            filterchainCutupTransitionTo = new VideoCutTo<TResource>(0D, durationLength);

            //an existing trim cannot be located, so a new one is required
            if (filterchainCutupBodyFrom == null)
            {
                filterchainCutupBodyFrom = new VideoCutTo<TResource>(0D, durationFromEndLength);
            }
            else
            {
                var trimFilter = filterchainCutupBodyFrom.Filters.Get<Trim>();
                trimFilter.End -= Duration.TotalSeconds;
                trimFilter.Duration = trimFilter.End - trimFilter.Start;
                filterchainCutupBodyFrom.Filters.Merge(trimFilter, FfmpegMergeOptionType.NewWins);
                durationFromEndLength = trimFilter.End;
                resourceFromLength += Duration.TotalSeconds;
            }
            filterchainCutupTransitionFrom = new VideoCutTo<TResource>(durationFromEndLength, resourceFromLength);

            //reset the filtergraph outputs
            if (resourceTo != null)
            {
                var newInputReceipt = command.RegenerateResourceMap(filterchain.Resources[1]);
                filterchainCutupBodyTo.SetResources(newInputReceipt);
                filterchainCutupTransitionTo.SetResources(newInputReceipt);
            }
            else
            {
                filterchainCutupTransitionTo.SetResources(filterchainCutupBodyTo.Resources.First());
            }
            if (resourceFrom != null)
            {
                var newInputReceipt = command.RegenerateResourceMap(filterchain.Resources[0]);
                filterchainCutupBodyFrom.SetResources(newInputReceipt);
                filterchainCutupTransitionFrom.SetResources(newInputReceipt);
            }
            else
            {
                filterchainCutupTransitionFrom.SetResources(filterchainCutupBodyFrom.Resources.First());
            }

            filterchainCutupBodyFrom.Output.Resource.Map = filterchain.Resources[0].Map;
            filterchainCutupBodyTo.Output.Resource.Map = filterchain.Resources[1].Map;

            command.Filtergraph.Merge(filterchainCutupBodyFrom, FfmpegMergeOptionType.NewWins);
            command.Filtergraph.Add(filterchainCutupTransitionFrom);
            command.Filtergraph.Add(filterchainCutupTransitionTo);
            command.Filtergraph.Merge(filterchainCutupBodyTo, FfmpegMergeOptionType.NewWins);

            var transitionToReceipt = new CommandResourceReceipt(command.Parent.Id, command.Id, filterchainCutupTransitionTo.Output.Resource.Map);
            var transitionFromReceipt = new CommandResourceReceipt(command.Parent.Id, command.Id, filterchainCutupTransitionFrom.Output.Resource.Map);
            var filterchainCopyTo = ResolutionFilterchain.Copy<TResource>();
            var filterchainCopyFrom = ResolutionFilterchain.Copy<TResource>();
            filterchainCopyTo.SetResources(transitionToReceipt);
            filterchainCopyFrom.SetResources(transitionFromReceipt);
            command.Filtergraph.Add(filterchainCopyTo);
            command.Filtergraph.Add(filterchainCopyFrom);

            //assign new receipts to the input filterchain
            var toReceipt = new CommandResourceReceipt(command.Parent.Id, command.Id, filterchainCopyTo.Output.Resource.Map);
            var fromReceipt = new CommandResourceReceipt(command.Parent.Id, command.Id, filterchainCopyFrom.Output.Resource.Map);
            filterchain.SetResources(toReceipt, fromReceipt);
        }
    }
}

namespace Hudl.Ffmpeg.Filters.Obsolete
{
    [Obsolete("AFade using Obsolete.BaseFilter is obsolete, use AFade reference.", false)]
    [AppliesToResource(Type = typeof(IAudio))]
    public class AFade : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "afade";

        public AFade()
            : base(FilterType, FilterMaxInputs)
        {
            Transition = FadeTransitionType.In;
            Unit = AudioUnitType.Seconds;
        }
        public AFade(FadeTransitionType transition, double duration)
            : this()
        {
            Transition = transition;
            Duration = duration;
        }
        public AFade(FadeTransitionType transition, double duration, double overrideStartAt)
            : this(transition, duration)
        {
            OverrideStartAt = overrideStartAt;
        }

        public FadeTransitionType Transition { get; set; } 

        public AudioUnitType Unit { get; set; }
        
        public double Duration { get; set; }

        public double? OverrideStartAt { get; set; }

        public override string ToString()
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Audio Fade must be greater than zero.");
            }

            var filter = new StringBuilder(100);
            var startAtLocation = 0d;
            if (OverrideStartAt.HasValue)
            {
                startAtLocation = OverrideStartAt.Value;
            }
            else if (Transition == FadeTransitionType.Out)
            {
                startAtLocation = CommandResources[0].Resource.Length.TotalSeconds - Duration; 
            }
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit) 
            {
                case AudioUnitType.Sample:
                    filter.AppendFormat(":ss={0}:ns={1}",
                        startAtLocation, 
                        Duration);
                    break;
                default : //seconds 
                    filter.AppendFormat(":st={0}:d={1}",
                        startAtLocation, 
                        Duration);
                    break;
            }
 
            return string.Concat(Type, "=", filter.ToString());
        }
    }

    [Obsolete("AMix using Obsolete.BaseFilter is obsolete, use AMix reference.", false)]
    [AppliesToResource(Type = typeof(IAudio))]
    public class AMix : BaseFilter
    {
        private const int FilterMaxInputs = 4;
        private const string FilterType = "amix";
        private const int AMixDropoutTransitionDefault = 2;

        public AMix()
            : base(FilterType, FilterMaxInputs)
        {
            DropoutTransition = AMixDropoutTransitionDefault;
            DurationType = DurationType.Longest;
        }
        public AMix(DurationType duration)
            : this()
        {
            DurationType = duration;
        }
        public AMix(DurationType duration, int dropoutTransition)
            : this(duration)
        {
            DropoutTransition = dropoutTransition;
        }

        public int DropoutTransition { get; set; }

        public DurationType DurationType { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            switch (DurationType)
            {
                case DurationType.First:
                    return resources.First().Resource.Length;
                case DurationType.Shortest:
                    return resources.Min(r => r.Resource.Length);
                default:
                    return resources.Max(r => r.Resource.Length);
            }
        }

        public override string ToString()
        {
            if (CommandResources.Count < 2)
            {
                throw new InvalidOperationException("Number of inputs cannot be less than defualt of 2");
            }
            if (DropoutTransition < 2)
            {
                throw new InvalidOperationException("Dropout transition cannot be less than default of 2");
            }

            //build the filter string 
            var filter = new StringBuilder(100);
            if (CommandResources.Count > 2)
            {
                filter.AppendFormat("{1}inputs={0}",
                    CommandResources.Count,
                    filter.Length > 0 ? ":" : "=");
            }
            if (DurationType != DurationType.Longest)
            {
                filter.AppendFormat("{1}duration={0}",
                    DurationType.ToString().ToLower(),
                    filter.Length > 0 ? ":" : "=");
            }
            if (DropoutTransition > 2)
            {
                filter.AppendFormat("{1}dropout_transition={0}",
                    DropoutTransition,
                    filter.Length > 0 ? ":" : "=");
            }

            //return the filter string information 
            return string.Concat(Type, filter.ToString());
        }
    }

    [Obsolete("AMovie using Obsolete.BaseFilter is obsolete, use AMovie reference.", false)]
    [AppliesToResource(Type = typeof(IAudio))]
    public class AMovie : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "amovie";

        public AMovie()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public AMovie(IAudio file)
            : this()
        {
            File = file;
        }

        public IAudio File { get; set; }

        public override string ToString()
        {
            if (File == null)
            {
                throw new InvalidOperationException("AMovie input cannot be nothing");
            }

            return string.Concat(Type, "=", File.Path);
        }
    }

    [Obsolete("Blend using Obsolete.BaseFilter is obsolete, use Blend reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class Blend : BaseFilter
    {
        private const int FilterMaxInputs = 2;
        private const string FilterType = "blend";

        public Blend()
            : base(FilterType, FilterMaxInputs)
        {
            Mode = BlendVideoModeType.and;
            Option = BlendVideoOptionType.all_expr;
        }
        public Blend(string expression)
            : this()
        {
            Expression = expression;
        }

        public BlendVideoOptionType Option { get; set; }

        public BlendVideoModeType Mode { get; set; }

        /// <summary>
        /// the blend expression details can be found at http://ffmpeg.org/ffmpeg-all.html#blend. 
        /// </summary>
        public string Expression { get; set; }

        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<CommandResource<IResource>> resources)
        {
            return resources.Min(r => r.Resource.Length);
        }

        public override string ToString()
        {
            if (Option == BlendVideoOptionType.all_expr && string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with Blend Option 'all_expr'");
            }

            var filter = new StringBuilder(100);
            filter.AppendFormat("{0}", Option.ToString());
            switch (Option)
            {
                case BlendVideoOptionType.all_expr:
                    filter.AppendFormat("='{0}'", Expression);
                    break;
                default:
                    filter.AppendFormat("={0}", Mode);
                    break;
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }

    [Obsolete("ColorBalance using Obsolete.BaseFilter is obsolete, use ColorBalance reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class ColorBalance : BaseFilter
    {
        private const int FilterMaxInputs = 2;
        private const string FilterType = "colorbalance";

        public ColorBalance()
            : base(FilterType, FilterMaxInputs)
        {
            Shadow = new FfmpegScaleRgb();
            Midtone = new FfmpegScaleRgb();
            Highlight = new FfmpegScaleRgb();
        }
        public ColorBalance(FfmpegScaleRgb shadows, FfmpegScaleRgb midtones, FfmpegScaleRgb highlights)
            : base(FilterType, FilterMaxInputs)
        {
            if (shadows == null)
            {
                throw new ArgumentNullException("shadows");
            }
            if (midtones == null)
            {
                throw new ArgumentNullException("midtones");
            }
            if (highlights == null)
            {
                throw new ArgumentNullException("highlights");
            }

            Shadow = shadows;
            Midtone = midtones;
            Highlight = highlights;
        }

        /// <summary>
        /// property to the RGB shadow color balancing
        /// </summary>
        public FfmpegScaleRgb Shadow { get; set; }

        /// <summary>
        /// property to the RGB midtone color balancing
        /// </summary>
        public FfmpegScaleRgb Midtone { get; set; }

        /// <summary>
        /// property to the RGB highlight color balancing
        /// </summary>
        public FfmpegScaleRgb Highlight { get; set; }

        public override string ToString()
        {
            if (Shadow.Red.Value == 0 &&
                Shadow.Green.Value == 0 &&
                Shadow.Blue.Value == 0 &&
                Midtone.Red.Value == 0 &&
                Midtone.Green.Value == 0 &&
                Midtone.Blue.Value == 0 &&
                Highlight.Red.Value == 0 &&
                Highlight.Green.Value == 0 &&
                Highlight.Blue.Value == 0)
            {
                throw new InvalidOperationException("At least one Color Balance ratio greater or less than 0 is required.");
            }

            var filter = new StringBuilder(100);
            if (Shadow.Red.Value != 0)
            {
                filter.AppendFormat("{1}rs={0}",
                    Shadow.Red,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Shadow.Green.Value != 0)
            {
                filter.AppendFormat("{1}gs={0}",
                    Shadow.Green,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Shadow.Blue.Value != 0)
            {
                filter.AppendFormat("{1}bs={0}",
                    Shadow.Blue,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Midtone.Red.Value != 0)
            {
                filter.AppendFormat("{1}rm={0}",
                    Midtone.Red,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Midtone.Blue.Value != 0)
            {
                filter.AppendFormat("{1}bm={0}",
                    Midtone.Blue,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Midtone.Green.Value != 0)
            {
                filter.AppendFormat("{1}gm={0}",
                    Midtone.Green,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Highlight.Red.Value != 0)
            {
                filter.AppendFormat("{1}rh={0}",
                    Highlight.Red,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Highlight.Green.Value != 0)
            {
                filter.AppendFormat("{1}gh={0}",
                    Highlight.Green,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Highlight.Blue.Value != 0)
            {
                filter.AppendFormat("{1}bh={0}",
                    Highlight.Blue,
                    (filter.Length > 0) ? ":" : string.Empty);
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }

    [Obsolete("Concat using Obsolete.BaseFilter is obsolete, use Concat reference.", false)]
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
    public class Concat : BaseFilter, IFilterValidator
    {
        private const int FilterMinInputs = 2;
        private const int FilterMaxInputs = 4;
        private const string FilterType = "concat";
        private const int DefaultVideoOut = 1;
        private const int DefaultAudioOut = 0;

        public Concat()
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfVideoOut = DefaultVideoOut;
            NumberOfAudioOut = DefaultAudioOut;
        }
        public Concat(int numberOfAudioOut, int numberOfVideoOut)
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfAudioOut = numberOfAudioOut;
            NumberOfVideoOut = numberOfVideoOut;
        }

        public int NumberOfVideoOut { get; set; }

        public int NumberOfAudioOut { get; set; }

        public override string ToString()
        {
            var numberOfResources = CommandResources.Count;
            if (NumberOfVideoOut > numberOfResources)
            {
                throw new InvalidOperationException("Number of Videos out cannot be greater than Resources in.");
            }
            if (NumberOfAudioOut > numberOfResources)
            {
                throw new InvalidOperationException("Number of Audios out cannot be greater than Resources in.");
            }

            var filter = new StringBuilder(100);
            if (numberOfResources > FilterMinInputs)
            {
                filter.AppendFormat("{1}n={0}",
                    numberOfResources,
                    (filter.Length > 0) ? ":" : "=");
            }
            if (NumberOfVideoOut != DefaultVideoOut)
            {
                filter.AppendFormat("{1}v={0}",
                    NumberOfVideoOut,
                    (filter.Length > 0) ? ":" : "=");
            }
            if (NumberOfAudioOut != DefaultAudioOut)
            {
                filter.AppendFormat("{1}a={0}",
                    NumberOfAudioOut,
                    (filter.Length > 0) ? ":" : "=");
            }

            return string.Concat(Type, filter.ToString());
        }

        public bool Validate(Command<IResource> command, Filterchain<IResource> filterchain, List<CommandResourceReceipt> resources)
        {
            //concat filters should be used independently of other filters
            return filterchain.Filters.Count > 1;
        }
    }

    [Obsolete("Fade using Obsolete.BaseFilter is obsolete, use Fade reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class Fade : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "fade";

        public Fade()
            : base(FilterType, FilterMaxInputs)
        {
            Transition = FadeTransitionType.In;
            Unit = VideoUnitType.Seconds;
        }
        public Fade(FadeTransitionType transition, double duration)
            : this()
        {
            Transition = transition;
            Duration = duration;
        }
        public Fade(FadeTransitionType transition, double duration, double overrideStartAt)
            : this(transition, duration)
        {
            OverrideStartAt = overrideStartAt;
        }

        public double Duration { get; set; }

        public double? OverrideStartAt { get; set; }

        public VideoUnitType Unit { get; set; }

        public FadeTransitionType Transition { get; set; }

        public override string ToString()
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Duration of the Video Fade cannot be zero.");
            }

            var filter = new StringBuilder(100);
            var startAtLocation = 0d;
            if (Transition == FadeTransitionType.Out)
            {
                startAtLocation = CommandResources[0].Resource.Length.TotalSeconds - Duration;
            }
            filter.AppendFormat("t={0}", Transition.ToString().ToLower());
            switch (Unit)
            {
                case VideoUnitType.Frames:
                    filter.AppendFormat(":s={0}:n={1}",
                        startAtLocation,
                        Duration);
                    break;
                default: //seconds 
                    filter.AppendFormat(":st={0}:d={1}",
                        startAtLocation,
                        Duration);
                    break;
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }

    [Obsolete("Movie using Obsolete.BaseFilter is obsolete, use Movie reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    public class Movie : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "movie";

        public Movie()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Movie(IResource file)
            : this()
        {
            File = file;
        }

        public IResource File { get; set; }

        public override string ToString()
        {
            if (File == null)
            {
                throw new InvalidOperationException("Movie input cannot be nothing");
            }

            return string.Concat(Type, "=", File.Path);
        }
    }

    [Obsolete("Overlay using Obsolete.BaseFilter is obsolete, use Overlay reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    public class Overlay : BaseFilter
    {
        private const int FilterMaxInputs = 2;
        private const string FilterType = "overlay";

        public Overlay()
            : base(FilterType, FilterMaxInputs)
        {
            Format = OverlayVideoFormatType.Yuv420;
            Eval = OverlayVideoEvalType.Frame;
        }

        public string X { get; set; }

        public string Y { get; set; }

        public bool Shortest { get; set; }

        public bool RepeatLast { get; set; }

        public OverlayVideoEvalType Eval { get; set; }

        public OverlayVideoFormatType Format { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return Shortest
                ? resources.Min(r => r.Resource.Length)
                : resources.Max(r => r.Resource.Length);
        }

        public override string ToString()
        {
            var filter = new StringBuilder(100);
            if (!string.IsNullOrWhiteSpace(X))
            {
                filter.AppendFormat("{1}x={0}",
                    X,
                    (filter.Length > 0) ? ":" : "=");
            }
            if (!string.IsNullOrWhiteSpace(Y))
            {
                filter.AppendFormat("{1}y={0}",
                    Y,
                    (filter.Length > 0) ? ":" : "=");
            }
            if (Eval != OverlayVideoEvalType.Frame)
            {
                filter.AppendFormat("{1}eval={0}",
                    Eval.ToString().ToLower(),
                    (filter.Length > 0) ? ":" : "=");
            }
            if (Format != OverlayVideoFormatType.Yuv420)
            {
                filter.AppendFormat("{1}format={0}",
                    Format.ToString().ToLower(),
                    (filter.Length > 0) ? ":" : "=");
            }
            if (Shortest)
            {
                filter.AppendFormat("{1}shortest={0}",
                    Convert.ToInt32(Shortest),
                    (filter.Length > 0) ? ":" : "=");
            }
            if (RepeatLast)
            {
                filter.AppendFormat("{1}repeatlast={0}",
                    Convert.ToInt32(RepeatLast),
                    (filter.Length > 0) ? ":" : "=");
            }

            return string.Concat(Type, filter.ToString());
        }
    }

    [Obsolete("Scale using Obsolete.BaseFilter is obsolete, use Scale reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class Scale : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "scale";

        public Scale()
            : base(FilterType, FilterMaxInputs)
        {
            Dimensions = new Point(0, 0);
        }
        public Scale(ScalePresetType preset)
            : this()
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Dimensions = scalingPresets[preset];
        }
        public Scale(int x, int y)
            : this()
        {
            if (x <= 0)
            {
                throw new ArgumentException("X must be greater than zero for scaling.");
            }
            if (y <= 0)
            {
                throw new ArgumentException("Y must be greater than zero for scaling.");
            }

            Dimensions = new Point(x, y);
        }

        public Point Dimensions { get; set; }

        public override string ToString()
        {
            if (Dimensions.X <= 0)
            {
                throw new InvalidOperationException("Dimensions.X must be greater than zero for scaling.");
            }
            if (Dimensions.Y <= 0)
            {
                throw new InvalidOperationException("Dimensions.Y must be greater than zero for scaling.");
            }

            return string.Concat(Type, "=w=", Dimensions.X, ":h=", Dimensions.Y);
        }
    }

    [Obsolete("SetDar using Obsolete.BaseFilter is obsolete, use SetDar reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class SetDar : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setdar";

        public SetDar()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public SetDar(FfmpegRatio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; }

        public override string ToString()
        {
            if (Ratio == null)
            {
                throw new InvalidOperationException("Ratio cannot be null.");
            }

            return string.Concat(Type, "=dar=", Ratio);
        }
    }

    [Obsolete("SetPts using Obsolete.BaseFilter is obsolete, use SetPts reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class SetPts : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setpts";
        private const string ResetPtsExpression = "PTS-STARTPTS";

        public SetPts()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public SetPts(string expression)
            : this()
        {
            Expression = expression;
        }
        public SetPts(bool resetTimestamp)
            : this(ResetPtsExpression)
        {
        }

        /// <summary>
        /// the setpts expression details can be found at http://ffmpeg.org/ffmpeg-all.html#setpts_002c-asetpts
        /// </summary>
        public string Expression { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with a set PTS filter");
            }

            return string.Concat(Type, "=", Expression);
        }
    }

    [Obsolete("SetSar using Obsolete.BaseFilter is obsolete, use SetSar reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class SetSar : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "setsar";

        public SetSar()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public SetSar(FfmpegRatio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentException("Ratio cannot be null.", "ratio");
            }

            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; }

        public override string ToString()
        {
            if (Ratio == null)
            {
                throw new InvalidOperationException("Ratio cannot be null.");
            }

            return string.Concat(Type, "=sar=", Ratio);
        }
    }

    [Obsolete("Trim using Obsolete.BaseFilter is obsolete, use Trim reference.", false)]
    [AppliesToResource(Type = typeof(IVideo))]
    public class Trim : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "trim";

        public Trim()
            : base(FilterType, FilterMaxInputs)
        {
            TimebaseUnit = VideoUnitType.Seconds;
        }
        public Trim(double startUnit, double endUnit, double duration, VideoUnitType timebaseUnit)
            : this()
        {
            if (startUnit < 0)
            {
                throw new ArgumentException("Start Unit cannot be less than zero", "startUnit");
            }
            if (endUnit < 0)
            {
                throw new ArgumentException("End Unit cannot be less than zero", "endUnit");
            }
            if (endUnit > 0D && endUnit <= startUnit)
            {
                throw new ArgumentException("End Unit cannot be less than Start Unit", "endUnit");
            }
            if (duration <= 0)
            {
                throw new ArgumentException("Duration of trimmed video must be greater than zero", "duration");
            }

            End = endUnit;
            Start = startUnit;
            Duration = duration;
            TimebaseUnit = timebaseUnit;
        }

        /// <summary>
        /// the start measure of where the video is to be trimmed to
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// the end measure of where the video is to be trimmed too
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// the maximum duration of the output (required)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// the base unit of measure for the trim command
        /// </summary>
        public VideoUnitType TimebaseUnit { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return TimeSpan.FromSeconds(Duration);
        }

        public override string ToString()
        {
            if (Duration <= 0)
            {
                throw new InvalidOperationException("Output duration cannot be empty for a video trim.");
            }

            var filter = new StringBuilder(100);
            switch (TimebaseUnit)
            {
                case VideoUnitType.Frames:
                    if (Start > 0)
                    {
                        filter.AppendFormat("{1}start_frame={0}",
                                Start,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    if (End > 0)
                    {
                        filter.AppendFormat("{1}end_frame={0}",
                                End,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    break;
                default:
                    if (Start > 0)
                    {
                        filter.AppendFormat("{1}start={0}",
                                Start,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    if (End > 0)
                    {
                        filter.AppendFormat("{1}end={0}",
                                End,
                                (filter.Length > 0) ? ":" : "=");
                    }
                    break;
            }

            filter.AppendFormat("{1}duration={0}",
                    Duration,
                    (filter.Length > 0) ? ":" : string.Empty);

            return string.Concat(Type, filter.ToString());
        }
    }

    [Obsolete("Volume using Obsolete.BaseFilter is obsolete, use Volume reference.", false)]
    [AppliesToResource(Type = typeof(IAudio))]
    public class Volume : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "volume";

        public Volume()
            : base(FilterType, FilterMaxInputs)
        {
            Scale = 1m;
        }
        public Volume(decimal scale)
            : this()
        {
            Scale = scale;
        }

        public decimal Scale { get; set; }

        public override string ToString()
        {
            if (Scale == 1m)
            {
                throw new InvalidOperationException("Scale has no effect at 100% of the current volume.");
            }

            return string.Concat(Type, "=volume=", Scale.ToString());
        }
    }
}

namespace Hudl.Ffmpeg.Templates.Obsolete.BaseTypes
{
    [Obsolete("BaseCommandFactoryTemplate is obsolete, use BaseCommandFactoryTemplate instead.", false)]
    public abstract class BaseCommandFactoryTemplate
    {
        protected BaseCommandFactoryTemplate(Command.CommandConfiguration configuration)
        {
            Factory = new Command.Obsolete.CommandFactory(configuration);       
            AudioList = new List<IAudio>();
            VideoList = new List<IVideo>();
            ImageList = new List<IImage>();
        }

        public void Add(IAudio resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            AudioList.Add(resource);
        }
        
        public void Add(IVideo resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            VideoList.Add(resource);
        }

        public void Add(IImage resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            ImageList.Add(resource);
        }

        public List<IResource> Render()
        {
            if (!SetupCompleted)
            {
                SetupTemplate();
                SetupCompleted = true;
            }

            return Factory.Render();
        }

        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            if (!SetupCompleted)
            {
                SetupTemplate();
                SetupCompleted = true;
            }

            return Factory.RenderWith<TProcessor>();
        }

        public List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : ICommandProcessor, new()
        {
            if (!SetupCompleted)
            {
                SetupTemplate();
                SetupCompleted = true;
            }

            return Factory.RenderWith(processor);
        }

        /// <summary>
        /// Called right before the Render process
        /// </summary>
        protected abstract void SetupTemplate();

        public List<IResource> GetAllWrittenFiles()
        {
            return Factory.GetAllOutput();
        }

        #region Internals
        internal protected bool SetupCompleted { get; protected set; }
        internal protected List<IVideo> VideoList { get; protected set; }
        internal protected List<IAudio> AudioList { get; protected set; }
        internal protected List<IImage> ImageList { get; protected set; }
        internal protected Command.Obsolete.CommandFactory Factory { get; protected set; }
        #endregion 
    }

    [Obsolete("BaseFilterchainAndSettingsTemplate is obsolete, use BaseFilterchainAndSettingsTemplatev2 instead.", false)]
    public abstract class BaseFilterchainAndSettingsTemplate<TOutput>
        where TOutput : IResource, new()
    {
        protected BaseFilterchainAndSettingsTemplate(SettingsCollectionResourceType collectionResourceType)
        {
            BaseFilterchain = Filterchain.FilterTo<TOutput>();
            switch (collectionResourceType)
            {
                case SettingsCollectionResourceType.Input:
                    BaseSettings = SettingsCollection.ForInput();
                    break;
                case SettingsCollectionResourceType.Output:
                    BaseSettings = SettingsCollection.ForOutput();
                    break;
                default:
                    throw new InvalidOperationException("Cannot add a setting template type of Any");
                    break;
            }
        }

        public static implicit operator Filterchain<TOutput>(BaseFilterchainAndSettingsTemplate<TOutput> filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseFilterchain;
        }

        public static implicit operator Filterchain<IResource>(BaseFilterchainAndSettingsTemplate<TOutput> filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseFilterchain;
        }

        public static implicit operator SettingsCollection(BaseFilterchainAndSettingsTemplate<TOutput> filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseSettings;
        }

        #region Internals
        internal protected SettingsCollection BaseSettings { get; protected set; }
        internal protected Filterchain<TOutput> BaseFilterchain { get; protected set; }
        #endregion
    }

    [Obsolete("BaseFilterchainTemplate is obsolete, use BaseFilterchainTemplatev2 instead.", false)]
    public abstract class BaseFilterchainTemplate<TOutput>
        where TOutput : IResource, new()
    {
        protected BaseFilterchainTemplate()
        {
            Base = Filterchain.FilterTo<TOutput>();
        }

        public static implicit operator Filterchain<TOutput>(BaseFilterchainTemplate<TOutput> filterchain)
        {
            return filterchain.Base;
        }

        public static implicit operator Filterchain<IResource>(BaseFilterchainTemplate<TOutput> filterchain)
        {
            return filterchain.Base;
        }

        #region Internals
        internal protected Filterchain<TOutput> Base { get; protected set; }
        #endregion
    }
}

namespace Hudl.Ffmpeg.Templates.Obsolete
{
    [Obsolete("Resolution1080P is obsolete, use Resolution1080P instead.", false)]
    public class Resolution1080P<TResource> : BaseFilterchainAndSettingsTemplate<TResource>
        where TResource : IVideo, new()
    {
        public Resolution1080P()
            : base(SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Hd1080),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Hd1080),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }
    }

    [Obsolete("Resolution720P is obsolete, use Resolution720P instead.", false)]
    public class Resolution720P<TResource> : BaseFilterchainAndSettingsTemplate<TResource>
        where TResource : IVideo, new()
    {
        public Resolution720P()
            : base(SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Hd720),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Hd720),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }
    }

    [Obsolete("Resolution480P is obsolete, use Resolution480P instead.", false)]
    public class Resolution480P<TResource> : BaseFilterchainAndSettingsTemplate<TResource>
        where TResource : IVideo, new()
    {
        public Resolution480P()
            : base(SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Hd480),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Hd480),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }
    }

    [Obsolete("Resolution360P is obsolete, use Resolution360P instead.", false)]
    public class Resolution360P<TResource> : BaseFilterchainAndSettingsTemplate<TResource>
        where TResource : IVideo, new()
    {
        public Resolution360P()
            : base(SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Sd360),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Sd360),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }
    }

    [Obsolete("Resolution240P is obsolete, use Resolution240P instead.", false)]
    public class Resolution240P<TResource> : BaseFilterchainAndSettingsTemplate<TResource>
        where TResource : IVideo, new()
    {
        public Resolution240P()
            : base(SettingsCollectionResourceType.Output)
        {
            BaseFilterchain.Filters.AddRange(
                new Scale(ScalePresetType.Sd240),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );

            BaseSettings.AddRange(SettingsCollection.ForOutput(
                new Dimensions(ScalePresetType.Sd240),
                new AspectRatio(new FfmpegRatio(16, 9)))
            );
        }
    }

    [Obsolete("VideoCutTo is obsolete, use VideoCutTo instead.", false)]
    public class VideoCutTo<TOutput> : BaseFilterchainTemplate<TOutput>
        where TOutput : IResource, new()
    {
        public VideoCutTo(double startTime, double endTime)
        {
            End = endTime;
            Start = startTime;
            Duration = endTime - startTime;
            TimebaseUnit = VideoUnitType.Seconds;

            SetUpTemplate();
        }
        public VideoCutTo(double startFrame, double endFrame, double duration)
        {
            End = startFrame;
            Start = endFrame;
            Duration = duration;
            TimebaseUnit = VideoUnitType.Frames;

            SetUpTemplate();
        }

        private double Start { get; set; }
        private double End { get; set; }
        private double Duration { get; set; }
        private VideoUnitType TimebaseUnit { get; set; }

        private void SetUpTemplate()
        {
            Base.Filters.AddRange(
                new Trim(Start, End, Duration, TimebaseUnit),
                new SetPts(true)
            );
        }
    }
}

namespace Hudl.Ffmpeg.Command 
{
    public partial class Command
    {
        [Obsolete("OutputTo is obsolete, use OutputTo with Commandv2 reference.", false)]
        public static Command<TOutput> OutputTo<TOutput>(Obsolete.CommandFactory parent)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, new TOutput(), true);
        }

        [Obsolete("OutputTo is obsolete, use OutputTo with Commandv2 reference.", false)]
        public static Command<TOutput> OutputTo<TOutput>(Obsolete.CommandFactory parent, bool export)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, new TOutput(), export);
        }

        [Obsolete("OutputTo is obsolete, use OutputTo with Commandv2 reference.", false)]
        public static Command<TOutput> OutputTo<TOutput>(Obsolete.CommandFactory parent, TOutput outputToUse)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, outputToUse, true);
        }

        [Obsolete("OutputTo is obsolete, use OutputTo with Commandv2 reference.", false)]
        public static Command<TOutput> OutputTo<TOutput>(Obsolete.CommandFactory parent, TOutput outputToUse, bool export)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, outputToUse, SettingsCollection.ForOutput(), export);
        }

        [Obsolete("OutputTo is obsolete, use OutputTo with Commandv2 reference.", false)]
        public static Command<TOutput> OutputTo<TOutput>(Obsolete.CommandFactory parent, TOutput outputToUse, SettingsCollection outputSettings)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, outputToUse, SettingsCollection.ForOutput(), true);
        }

        [Obsolete("OutputTo is obsolete, use OutputTo with Commandv2 reference.", false)]
        public static Command<TOutput> OutputTo<TOutput>(Obsolete.CommandFactory parent, TOutput outputToUse, SettingsCollection outputSettings, bool export)
            where TOutput : IResource, new()
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (outputToUse == null)
            { 
                throw new ArgumentNullException("outputToUse");
            }
            if (outputSettings == null)
            {
                throw new ArgumentNullException("outputSettings");
            }

            return new Command<TOutput>(parent, outputToUse, outputSettings, export); 
        }
    }
}

namespace Hudl.Ffmpeg.Command.Obsolete
{
    [Obsolete("CommandBuilder is obsolete, do not reference Obsolete namespace.", false)]
    internal class CommandBuilder
    {
        private const string FfmpegMethodName = "ffmpeg";
        private readonly StringBuilder _builderBase;

        public CommandBuilder()
        {
            _builderBase = new StringBuilder(100);            
        }

        public void WriteCommand(Command<IResource> command)
        {
            command.ResourceList.ForEach(WriteResource);

            WriteFiltergraph(command, command.Filtergraph);

            WriteOutput(command.Output);

            WriteFinish();
        }
        private void WriteResource(CommandResource<IResource> resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var settingsData = Common.Obsolete.Validate.GetSettingCollectionData(resource.Settings);

            WriteResourcePreSettings(resource, settingsData);

            var inputResource = new Input(resource.Resource);
            _builderBase.Append(" ");
            _builderBase.Append(inputResource);

            WriteResourcePostSettings(resource, settingsData);
        }
        private void WriteResourcePreSettings(CommandResource<IResource> resource, Dictionary<Type, Settings.BaseTypes.SettingsApplicationData> settingsData)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Settings.SettingsList.ForEach(setting =>
            {
                var settingInfoData = settingsData[setting.GetType()];
                if (settingInfoData == null) return;
                if (!settingInfoData.PreDeclaration) return;
                if (settingInfoData.ResourceType != SettingsCollectionResourceType.Input) return;

                _builderBase.Append(" ");
                _builderBase.Append(setting);
            });
        }
        private void WriteResourcePostSettings(CommandResource<IResource> resource, Dictionary<Type, Settings.BaseTypes.SettingsApplicationData> settingsData)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Settings.SettingsList.ForEach(setting =>
            {
                var settingInfoData = settingsData[setting.GetType()];
                if (settingInfoData == null) return;
                if (settingInfoData.PreDeclaration) return;
                if (settingInfoData.ResourceType != SettingsCollectionResourceType.Input) return;

                _builderBase.Append(" ");
                _builderBase.Append(setting);
            });

        }
        private void WriteFiltergraph(Command<IResource> command, Filtergraph filtergraph)
        {
            if (filtergraph == null)
            {
                throw new ArgumentNullException("filtergraph");
            }

            var shouldIncludeDelimitor = false;
            filtergraph.FilterchainList.ForEach(filterchain =>
            {
                if (shouldIncludeDelimitor)
                {
                    _builderBase.Append(";");
                }
                else
                {
                    _builderBase.Append(" -filter_complex \"");
                    shouldIncludeDelimitor = true;
                }

                WriteFilterchain(command, filterchain);
            });

            if (shouldIncludeDelimitor)
            {
                _builderBase.Append("\"");
            }
        }
        private void WriteFilterchain(Command<IResource> command, Filterchain<IResource> filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            WriteFilterchainIn(command, filterchain);

            var shouldIncludeDelimitor = false;
            filterchain.Filters.List.ForEach(filter =>
            {
                if (shouldIncludeDelimitor)
                {
                    _builderBase.Append(",");
                }
                else
                {
                    _builderBase.Append(" ");
                    shouldIncludeDelimitor = true;
                }

                filter.Setup(command, filterchain);
                WriteFilter(filter);
            });

            WriteFilterchainOut(command, filterchain);
        }
        private void WriteFilterchainIn(Command<IResource> command, Filterchain<IResource> filterchain)
        {
            filterchain.ResourceList.ForEach(resource =>
            {
                _builderBase.Append(" ");
                var indexOfResource = command.ResourceList.FindIndex(r => r.Resource.Map == resource.Map);
                if (indexOfResource >= 0)
                {
                    var commandResource = command.ResourceList[indexOfResource];
                    _builderBase.Append(Formats.Map(commandResource.Resource, indexOfResource));
                }
                else
                {
                    _builderBase.Append(Formats.Map(resource.Map));
                }
            });
        }
        private void WriteFilterchainOut(Command<IResource> command, Filterchain<IResource> filterchain)
        {
            var filterchainIndex =
                command.Filtergraph.FilterchainList.FindIndex(
                    f => f.Output.Resource.Map == filterchain.Output.Resource.Map);
            if (filterchainIndex < command.Filtergraph.Count - 1)
            {
                _builderBase.Append(" ");
                _builderBase.Append(Formats.Map(filterchain.Output.Resource));
            }
        }
        private void WriteOutput(CommandOutput<IResource> output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var settingsData = Common.Obsolete.Validate.GetSettingCollectionData(output.Settings);

            WriteOutputSettings(output);

            _builderBase.AppendFormat(" {0}", Helpers.EscapePath(output.GetOutput()));
        }
        private void WriteOutputSettings(CommandOutput<IResource> output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var settingsData = Common.Obsolete.Validate.GetSettingCollectionData(output.Settings);
            output.Settings.SettingsList.ForEach(setting =>
            {
                var settingInfoData = settingsData[setting.GetType()];
                if (settingInfoData == null) return;
                if (!settingInfoData.PreDeclaration) return;
                if (settingInfoData.ResourceType != SettingsCollectionResourceType.Output) return;

                _builderBase.Append(" ");
                _builderBase.Append(setting);
            });
        }

        private void WriteFinish()
        {
            _builderBase.AppendLine();
        }
        private void WriteFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            _builderBase.Append(filter.ToString());
        }

        public override string ToString()
        {
            return _builderBase.ToString();
        }
    }

    [Obsolete("CommandBuilder is obsolete, do not reference Obsolete namespace.", false)]
    public class CommandFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CommandFactory).Name);

        public CommandFactory(CommandConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            Id = Guid.NewGuid().ToString();
            Configuration = configuration;
            CommandList = new List<Command<IResource>>();
        }

        /// <summary>
        /// Returns the count of commands that are in the factory, excluding prep commands
        /// </summary>
        public int Count { get { return CommandList.Count; } }

        /// <summary>
        /// Returns the path location for the command factory to store the output files.
        /// </summary>
        public CommandConfiguration Configuration { get; private set; }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddToOutput(Command<IResource> command)
        {
            command.Output.Resource.Path = Configuration.OutputPath;
            return Add(command, true);
        }

        /// <summary>
        /// Adds a new command and marks the output to be exported.
        /// </summary>
        public CommandFactory AddToResources(Command<IResource> command)
        {
            command.Output.Resource.Path = Configuration.TempPath;
            return Add(command, false);
        }

        /// <summary>
        /// Select the output resources for the current command factory 
        /// </summary>
        public List<IResource> GetOutput()
        {
            return CommandList.Where(c => c.Output.IsExported)
                              .Select(c => c.Output.Resource).ToList();
        }

        /// <summary>
        /// Select the output and temp resources for the current command factory 
        /// </summary>
        public List<IResource> GetAllOutput()
        {
            return CommandList.SelectMany(c =>
                {
                    var outputTempList = new List<IResource>();
                    outputTempList.Add(c.Output.Resource);
                    var prepTempList = c.CommandList.Select(pc => pc.Output.Resource).ToList();
                    if (prepTempList.Count > 0)
                    {
                        outputTempList.AddRange(prepTempList);
                    }
                    return outputTempList;
                }).ToList();
        }

        /// <summary>
        /// Returns a boolean indicating if the command already exists in the factory
        /// </summary>
        public bool Contains<TOutput>(Command<TOutput> command)
            where TOutput : IResource
        {
            return CommandList.Any(c => c.Id == command.Id);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public List<IResource> Render()
        {
            return RenderWith<CmdProcessorReciever>();
        }

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open(Configuration))
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        /// <summary>
        /// Renders the command stream with an existing command processor
        /// </summary>
        public List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : ICommandProcessor
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            var outputList = GetOutput();
            var commandCount = CommandList.Sum(c => 1 + c.CommandList.Count); 

            Log.InfoFormat("Rendering command factory Outputs={0} Commands={1}", 
                outputList.Count,
                commandCount);

            CommandList.ForEach(command =>
            {
                command.CommandList.ForEach(prepCommand => prepCommand.RenderWith(processor));

                command.RenderWith(processor);
            });

            return outputList;
        }

        #region Command Helper Visibility
        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>()
            where TOutput : class, IResource, new()
        {
            var temporaryResource = new TOutput();
            return CreateOutput<TOutput>(SettingsCollection.ForOutput(), temporaryResource.Name);
        }

        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>(string fileName)
            where TOutput : class, IResource, new()
        {
            return CreateOutput<TOutput>(SettingsCollection.ForOutput(), fileName);
        }

        /// <summary>
        /// Adds a new command using TOutput as a new instance
        /// </summary>
        public Command<TOutput> CreateOutput<TOutput>(SettingsCollection settings, string fileName)
            where TOutput : class, IResource, new()
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (string.IsNullOrWhiteSpace("fileName"))
            {
                throw new ArgumentException("Output file name cannot be empty", "fileName");
            }

            var newResource = Resource.Create<TOutput>(Configuration.OutputPath, fileName);
            return Command.OutputTo(this, newResource, settings);
        }
        #endregion 

        #region Internals
        internal string Id { get; set; }
        internal List<Command<IResource>> CommandList { get; set; }
        #endregion

        #region Utility
        private CommandFactory Add(Command<IResource> command, bool export)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (Contains(command))
            {
                throw new ArgumentException("Command Factory already contains this command.", "command");
            }
            if (command.Parent.Id != Id)
            {
                throw new ArgumentException("Command was not created as a child of this factory.", "command");
            }

            command.Output.IsExported = export;
            CommandList.Add(command);
            return this;
        }
        #endregion 
    }

    [Obsolete("Command<TOutput> is obsolete, do not reference Obsolete namespace.", false)]
    public class Command<TOutput>
       where TOutput : IResource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Command<TOutput>).Name);

        private Command(CommandFactory parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            Id = Guid.NewGuid().ToString();
            Parent = parent;
            Filtergraph = new Filtergraph();
            CommandList = new List<Command<IResource>>();
            ResourceList = new List<CommandResource<IResource>>();
        }
        internal Command(CommandFactory parent, TOutput outputToUse)
            : this(parent, outputToUse, SettingsCollection.ForOutput())
        {
            Output = new CommandOutput<TOutput>(this, outputToUse);
        }
        internal Command(CommandFactory parent, TOutput outputToUse, SettingsCollection outputSettings)
            : this(parent, outputToUse, outputSettings, true)
        {
            Output = new CommandOutput<TOutput>(this, outputToUse, outputSettings);
        }
        internal Command(CommandFactory parent, TOutput outputToUse, SettingsCollection outputSettings, bool export)
            : this(parent)
        {
            if (outputToUse == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (outputSettings == null)
            {
                throw new ArgumentNullException("outputSettings");
            }

            Output = new CommandOutput<TOutput>(this, outputToUse, outputSettings, export);
        }

        #region Obsolete

        [Obsolete("Outputs are not always singular from ffmpeg, you should handle this appropriately. use Outputs")]
        public CommandOutput<TOutput> Output { get; protected set; }

        #endregion

        public List<CommandOutput<TOutput>> Outputs { get; protected set; }


        public TimeSpan Length { get { return TimeSpan.FromSeconds(Helpers.GetLength(this)); } }

        public ReadOnlyCollection<CommandResource<IResource>> Resources { get { return ResourceList.AsReadOnly(); } }

        public ReadOnlyCollection<Filterchain<IResource>> Filterchains { get { return Filtergraph.FilterchainList.AsReadOnly(); } }

        public static implicit operator Command<IResource>(Command<TOutput> command)
        {
            var commandNew = new Command<IResource>(command.Parent,
                command.Output.Resource,
                command.Output.Settings,
                command.Output.IsExported);
            commandNew.Id = command.Id;
            commandNew.Filtergraph = command.Filtergraph;
            if (command.ResourceList.Count > 0)
            {
                commandNew.ResourceList = new List<CommandResource<IResource>>(command.ResourceList);
            }
            if (command.CommandList.Count > 0)
            {
                commandNew.CommandList = new List<Command<IResource>>(command.CommandList);
            }
            return commandNew;
        }

        public bool Contains(CommandResourceReceipt receipt)
        {
            if (receipt.FactoryId == Parent.Id && receipt.CommandId == Id)
            {
                return ResourceList.Any(r => r.Resource.Map == receipt.Map);
            }
            return false;
        }

        public bool Contains(IResource resource)
        {
            return ResourceList.Any(r => r.Resource.Map == resource.Map);
        }

        public CommandResourceReceipt AddResource<TResource>(string fullName)
            where TResource : class, IResource, new()
        {
            return AddResource<TResource>(SettingsCollection.ForInput(), fullName, TimeSpan.FromSeconds(0));
        }

        public CommandResourceReceipt AddResource<TResource>(string fullName, TimeSpan length)
            where TResource : class, IResource, new()
        {
            return AddResource<TResource>(SettingsCollection.ForInput(), fullName, length);
        }

        public CommandResourceReceipt AddResource<TResource>(SettingsCollection settings, string fullName)
            where TResource : class, IResource, new()
        {
            return AddResource<TResource>(settings, fullName, TimeSpan.FromSeconds(0));
        }

        public CommandResourceReceipt AddResource<TResource>(SettingsCollection settings, string fullName, TimeSpan length)
            where TResource : class, IResource, new()
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Resource full name cannot be null or empty.", "fullName");
            }

            var newResource = Resource.Create<TResource>(fullName, length);
            return Add(settings, newResource);
        }

        public CommandResourceReceipt AddAsset<TResource>(string fileName)
            where TResource : class, IResource, new()
        {
            return AddAsset<TResource>(SettingsCollection.ForInput(), fileName, TimeSpan.FromSeconds(0));
        }

        public CommandResourceReceipt AddAsset<TResource>(string fileName, TimeSpan length)
            where TResource : class, IResource, new()
        {
            return AddAsset<TResource>(SettingsCollection.ForInput(), fileName, length);
        }

        public CommandResourceReceipt AddAsset<TResource>(SettingsCollection settings, string fileName)
            where TResource : class, IResource, new()
        {
            return AddAsset<TResource>(settings, fileName, TimeSpan.FromSeconds(0));
        }

        public CommandResourceReceipt AddAsset<TResource>(SettingsCollection settings, string fileName, TimeSpan length)
            where TResource : class, IResource, new()
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Resource name cannot be null or empty.", "fileName");
            }

            var newResource = Resource.Create<TResource>(Parent.Configuration.AssetsPath, fileName, length);
            return Add(settings, newResource);
        }

        public CommandResourceReceipt Add(IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return Add(SettingsCollection.ForInput(), resource);
        }

        public CommandResourceReceipt Add(SettingsCollection settings, IResource resource)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (Contains(resource))
            {
                throw new ArgumentException("Command already contains the specified resource.", "resource");
            }
            if (settings.Type != SettingsCollectionResourceType.Input)
            {
                throw new ArgumentException("Command input cannot be given an output settings collection.", "settings");
            }

            var commandResource = new CommandResource<IResource>(this, settings, resource);
            ResourceList.Add(commandResource);
            return commandResource.GetReciept();
        }

        public List<CommandResourceReceipt> AddRange(List<IResource> resourceList)
        {
            if (resourceList == null)
            {
                throw new ArgumentNullException("resourceList");
            }

            return resourceList.Select(Add).ToList();
        }

        public List<CommandResourceReceipt> AddRange(Dictionary<SettingsCollection, IResource> resourceDictionary)
        {
            if (resourceDictionary == null)
            {
                throw new ArgumentNullException("resourceDictionary");
            }
            if (resourceDictionary.Any(kv => kv.Key.Type == SettingsCollectionResourceType.Output))
            {
                throw new ArgumentException("Range of resources contains settings output collection.", "resourceDictionary");
            }

            return resourceDictionary.Select(r => Add(r.Key, r.Value)).ToList();
        }

        public CommandResourceReceipt Insert(int index, IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return Insert(index, resource, SettingsCollection.ForInput());
        }

        public CommandResourceReceipt Insert(int index, IResource resource, SettingsCollection settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (Contains(resource))
            {
                throw new ArgumentException("Command already contains the specified resource.", "resource");
            }
            if (settings.Type != SettingsCollectionResourceType.Input)
            {
                throw new ArgumentException("Command input cannot be given an output settings collection.", "settings");
            }

            var commandResource = new CommandResource<IResource>(this, settings, resource);
            ResourceList.Insert(index, commandResource);
            return commandResource.GetReciept();
        }

        public CommandResourceReceipt Replace(CommandResourceReceipt replace, IResource replaceWith)
        {
            if (replace == null)
            {
                throw new ArgumentNullException("replace");
            }
            if (replaceWith == null)
            {
                throw new ArgumentNullException("replaceWith");
            }

            return Replace(replace, replaceWith, SettingsCollection.ForInput());
        }

        public CommandResourceReceipt Replace(CommandResourceReceipt replace, IResource replaceWith, SettingsCollection settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (replace == null)
            {
                throw new ArgumentNullException("replace");
            }
            if (replaceWith == null)
            {
                throw new ArgumentNullException("replaceWith");
            }
            if (Contains(replaceWith))
            {
                throw new ArgumentException("Command already contains the specified resource.", "replaceWith");
            }
            if (!Contains(replace))
            {
                throw new ArgumentException("Command does not contain the resource to replace.", "replace");
            }
            if (settings.Type != SettingsCollectionResourceType.Input)
            {
                throw new ArgumentException("Command input cannot be given an output settings collection.", "settings");
            }

            var commandResource = new CommandResource<IResource>(this, settings, replaceWith);
            var replaceIndex = ResourceList.FindIndex(c => c.Resource.Map == replace.Map);
            commandResource.Resource.Map = replace.Map;
            ResourceList.RemoveAt(replaceIndex);
            ResourceList.Insert(replaceIndex, commandResource);
            return commandResource.GetReciept();
        }

        public CommandResourceReceipt GetResourceReceipt(Func<CommandResource<IResource>, bool> predicate = null)
        {
            return GetResourceReceipts(predicate).FirstOrDefault();
        }

        public List<CommandResourceReceipt> GetResourceReceipts(Func<CommandResource<IResource>, bool> predicate = null)
        {
            if (predicate == null)
            {
                return ResourceList.Select(r => r.GetReciept())
                               .ToList();
            }
            return ResourceList.Where(predicate)
                               .Select(r => r.GetReciept())
                               .ToList();
        }

        public CommandResourceReceipt GetFilterchainReceipt(Func<CommandResourceReceipt, bool> predicate = null)
        {
            return GetFilterchainReceipts(predicate).FirstOrDefault();
        }

        public List<CommandResourceReceipt> GetFilterchainReceipts(Func<CommandResourceReceipt, bool> predicate = null)
        {
            if (predicate == null)
            {
                return Filtergraph.FilterchainList.SelectMany(r => r.ResourceList)
                                                  .ToList();
            }
            return Filtergraph.FilterchainList.SelectMany(r => r.ResourceList)
                                              .Where(predicate)
                                              .ToList();
        }

        public CommandResourceReceipt GetAllReceipt(Func<CommandResourceReceipt, bool> predicate = null)
        {
            return GetAllReceipts(predicate).FirstOrDefault();
        }

        public List<CommandResourceReceipt> GetAllReceipts(Func<CommandResourceReceipt, bool> predicate = null)
        {
            var resourceReceipts = GetResourceReceipts();
            var filterchainReceipts = GetFilterchainReceipts();
            var listToSearch = new List<CommandResourceReceipt>();
            if (resourceReceipts.Count > 0)
            {
                listToSearch.AddRange(resourceReceipts);
            }
            if (filterchainReceipts.Count > 0)
            {
                listToSearch.AddRange(filterchainReceipts);
            }

            return predicate == null
                ? listToSearch
                : listToSearch.Where(predicate).ToList();
        }

        public Command<TOutput> ApplySettings<TSetting>(TSetting setting, params CommandResourceReceipt[] resources)
            where TSetting : ISetting
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            return ApplySettings(SettingsCollection.ForInput(setting), resources);
        }

        public Command<TOutput> ApplySettings(SettingsCollection settings, params CommandResourceReceipt[] resources)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var resourceList = new List<CommandResourceReceipt>(resources);
            if (resourceList.Count == 0)
            {
                resourceList = ResourceList.Select(r => r.GetReciept()).ToList();
            }

            return ApplySettings(settings, resourceList);
        }

        public Command<TOutput> ApplySettings(SettingsCollection settings, List<CommandResourceReceipt> resources)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var resourceMaps = resources.Select(r => r.Map).ToList();
            ResourceList.Where(r => resourceMaps.Count == 0 || resourceMaps.Contains(r.Resource.Map))
                        .ToList()
                        .ForEach(r => r.Settings.AddRange(settings));

            return this;
        }

        public CommandResourceReceipt ApplyFilter<TResource, TFilter>()
            where TResource : IResource, new()
            where TFilter : IFilter, new()
        {
            return ApplyFilter<TResource, TFilter>(new TFilter());
        }

        public CommandResourceReceipt ApplyFilter<TResource, TFilter>(TFilter filter, params CommandResourceReceipt[] resources)
            where TResource : IResource, new()
            where TFilter : IFilter, new()
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            return ApplyFilter(Filterchain.FilterTo<TResource>(filter), resources);
        }

        public CommandResourceReceipt ApplyFilter<TResource>(Filterchain<TResource> filterchain, params CommandResourceReceipt[] resources)
            where TResource : IResource, new()
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var resourceList = new List<CommandResourceReceipt>(resources);
            if (resourceList.Count == 0)
            {
                resourceList = GetAllUnassignedReceipts();
            }

            return ApplyFilter(filterchain, resourceList);
        }

        public CommandResourceReceipt ApplyFilter<TResource>(Filterchain<TResource> filterchain, List<CommandResourceReceipt> resources)
            where TResource : IResource, new()
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (!ValidateFiltersMax(filterchain, resources))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, exceeds maximum calculated allowable resources.");
            }
            if (!ValidateFilters(filterchain, resources))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, failed to comply with child filter requirements.");
            }

            CommandResourceReceipt lastFilterchainReceipt = null;
            var maximumAllowed = GetFiltersMax(filterchain);
            var breakouts = Helpers.BreakReceipts(maximumAllowed, resources.ToArray());
            breakouts.ForEach(segment =>
            {
                var segmentList = new List<CommandResourceReceipt>(segment);
                var filterchainCopy = filterchain.Copy<TResource>();
                if (lastFilterchainReceipt != null)
                {
                    segmentList.Insert(0, lastFilterchainReceipt);
                }
                filterchainCopy.SetResources(segmentList);
                ProcessFilters(filterchainCopy);
                Filtergraph.Add(filterchainCopy);
                lastFilterchainReceipt = new CommandResourceReceipt(Parent.Id, Id, filterchainCopy.Output.Resource.Map);
            });

            return lastFilterchainReceipt;
        }

        public List<CommandResourceReceipt> ApplyFilterToEach<TResource, TFilter>()
            where TResource : IResource, new()
            where TFilter : IFilter, new()
        {
            return ApplyFilterToEach<TResource, TFilter>(new TFilter());
        }

        public List<CommandResourceReceipt> ApplyFilterToEach<TResource, TFilter>(TFilter filter, params CommandResourceReceipt[] resources)
            where TResource : IResource, new()
            where TFilter : IFilter, new()
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            return ApplyFilterToEach(Filterchain.FilterTo<TResource>(filter), resources);
        }

        public List<CommandResourceReceipt> ApplyFilterToEach<TResource>(Filterchain<TResource> filterchain, params CommandResourceReceipt[] resources)
            where TResource : IResource, new()
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var resourceList = new List<CommandResourceReceipt>(resources);
            if (resourceList.Count == 0)
            {
                resourceList = GetAllUnassignedReceipts();
            }

            return ApplyFilterToEach(filterchain, resourceList);
        }

        public List<CommandResourceReceipt> ApplyFilterToEach<TResource>(Filterchain<TResource> filterchain, List<CommandResourceReceipt> resources)
            where TResource : IResource, new()
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            return resources.Select(r => ApplyFilter(filterchain, r)).ToList();
        }



        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public CommandOutput<TOutput> Render()
        {
            return RenderWith<CmdProcessorReciever>();
        }

        /// <summary>
        /// Renders the command stream with a new command processor
        /// </summary>
        public CommandOutput<TOutput> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            var commandProcessor = new TProcessor();

            if (!commandProcessor.Open(Parent.Configuration))
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return returnType;
        }

        /// <summary>
        /// Renders the command stream with an existing command processor
        /// </summary>
        public CommandOutput<TOutput> RenderWith<TProcessor>(TProcessor commandProcessor)
            where TProcessor : ICommandProcessor
        {
            if (commandProcessor == null)
            {
                throw new ArgumentNullException("commandProcessor");
            }

            var commandBuilder = new CommandBuilder();
            commandBuilder.WriteCommand(this);

            if (!commandProcessor.Send(commandBuilder.ToString()))
            {
                throw new FfmpegRenderingException(commandProcessor.Error);
            }

            return Output;
        }

        #region Internals
        internal string Id { get; set; }
        internal CommandFactory Parent { get; set; }
        internal Filtergraph Filtergraph { get; set; }
        internal List<Command<IResource>> CommandList { get; set; }
        internal List<CommandResource<IResource>> ResourceList { get; set; }
        internal List<CommandResource<IResource>> CommandOnlyResourcesFromReceipts(params CommandResourceReceipt[] receipts)
        {
            return CommandOnlyResourcesFromReceipts(new List<CommandResourceReceipt>(receipts));
        }
        internal List<CommandResource<IResource>> CommandOnlyResourcesFromReceipts(List<CommandResourceReceipt> receipts)
        {
            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }

            var receiptMaps = receipts.Select(d => d.Map).ToList();
            return ResourceList.Where(r => receiptMaps.Contains(r.Resource.Map)).ToList();
        }
        internal List<CommandResource<IResource>> ResourcesFromReceipts(params CommandResourceReceipt[] receipts)
        {
            return ResourcesFromReceipts(new List<CommandResourceReceipt>(receipts));
        }
        internal List<CommandResource<IResource>> ResourcesFromReceipts(List<CommandResourceReceipt> receipts)
        {
            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }

            var receiptMaps = receipts.Select(d => d.Map).ToList();
            var outputList = new List<CommandResource<IResource>>();
            var commandResources = ResourceList.Where(r => receiptMaps.Contains(r.Resource.Map)).ToList();
            var filterchainResources = Filtergraph.FilterchainList.Where(f => receiptMaps.Contains(f.Output.Resource.Map))
                                                                  .Select(f => new CommandResource<IResource>(this, f.GetOutput(this).GetOutput()))
                                                                  .ToList();
            if (commandResources.Count > 0)
            {
                outputList.AddRange(commandResources);
            }
            if (filterchainResources.Count > 0)
            {
                outputList.AddRange(filterchainResources);
            }
            return outputList;
        }
        internal List<CommandResource<IResource>> PrepResourcesFromReceipts(params CommandResourceReceipt[] receipts)
        {
            return PrepResourcesFromReceipts(new List<CommandResourceReceipt>(receipts));
        }
        internal List<CommandResource<IResource>> PrepResourcesFromReceipts(List<CommandResourceReceipt> receipts)
        {
            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }

            var receiptMaps = receipts.Select(d => d.Map);
            return CommandList.Where(c => receiptMaps.Contains(c.Output.Resource.Map))
                              .Select(r => r.ResourceList.FirstOrDefault())
                              .ToList();
        }
        internal Command<IResource> PrepCommandFromReceipt(CommandResourceReceipt receipt)
        {
            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }

            return CommandList.FirstOrDefault(c => receipt.Map == c.Output.Resource.Map);
        }
        internal Filterchain<IResource> FilterchainFromReceipt(CommandResourceReceipt receipt)
        {
            if (receipt == null)
            {
                throw new ArgumentNullException("receipt");
            }

            return Filtergraph.FilterchainList.FirstOrDefault(f => f.Output.Resource.Map == receipt.Map);
        }
        internal CommandResource<IResource> GetCommandResource(CommandResourceReceipt receipt)
        {
            return ResourcesFromReceipts(receipt).FirstOrDefault();
        }
        internal CommandResourceReceipt RegenerateResourceMap(CommandResourceReceipt receipt)
        {
            if (receipt.FactoryId != Parent.Id ||
                receipt.CommandId != Id)
            {
                throw new InvalidOperationException("Receipt is not a part of this command.");
            }

            var resource = ResourceList.FirstOrDefault(r => r.Resource.Map == receipt.Map);
            if (resource == null)
            {
                throw new InvalidOperationException("Receipt is not a part of this command.");
            }

            resource.Resource.Map = Helpers.NewMap();

            return resource.GetReciept();
        }

        #endregion

        #region Utility
        private int GetFiltersMax<TResource>(Filterchain<TResource> filterchain)
            where TResource : IResource
        {
            return filterchain.Filters.List.Min(f => f.MaxInputs);
        }
        private bool ValidateFilters<TResource>(Filterchain<TResource> filterchain, List<CommandResourceReceipt> resources)
            where TResource : IResource, new()
        {
            return filterchain.Filters.List.TrueForAll(f =>
            {
                if (!(f is IFilterValidator))
                {
                    return true;
                }
                return (f as IFilterValidator).Validate(this, filterchain, resources);
            });
        }
        private bool ValidateFiltersMax<TResource>(Filterchain<TResource> filterchain, List<CommandResourceReceipt> resources)
            where TResource : IResource
        {
            var maximumAllowedMinimum = GetFiltersMax(filterchain);
            return maximumAllowedMinimum > 1 || (maximumAllowedMinimum == 1 && resources.Count == 1);
        }
        private void ProcessFilters<TResource>(Filterchain<TResource> filterchain)
            where TResource : IResource, new()
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            filterchain.Filters.List.ForEach(filter =>
            {
                if (!(filter is IFilterProcessor)) return;
                (filter as IFilterProcessor).PrepCommands(this, filterchain);
            });
        }
        private List<CommandResourceReceipt> GetAllUnassignedReceipts()
        {
            var unassignedReceipts = new List<CommandResourceReceipt>();

            //process all of the command resources first, commands must be unassigned to filters
            var commandReceipts =
                ResourceList.Where(
                    resource =>
                    !Filtergraph.FilterchainList.Any(
                        filterchain => filterchain.ResourceList.Any(r => r.Map == resource.Resource.Map)))
                            .Select(resource => resource.GetReciept())
                            .ToList();

            //process all filterchain outputs second, filterchains must be unassigned to filters
            var filterchainReceipts =
                Filtergraph.FilterchainList.Where(
                    filterchain =>
                    !Filtergraph.FilterchainList.Any(
                        f => f.ResourceList.Any(r => r.Map == filterchain.Output.Resource.Map)))
                            .Select(f => new CommandResourceReceipt(Parent.Id, Id, f.Output.Resource.Map))
                            .ToList();

            if (commandReceipts.Count > 0)
            {
                unassignedReceipts.AddRange(commandReceipts);
            }
            if (filterchainReceipts.Count > 0)
            {
                unassignedReceipts.AddRange(filterchainReceipts);
            }

            return unassignedReceipts;
        }
        #endregion

    }

    [Obsolete("CommandOutput<TOutput> is obsolete, do not reference Obsolete namespace.", false)]
    public class CommandOutput<TResource>
       where TResource : IResource
    {
        internal CommandOutput(Command<TResource> parent, TResource outputToUse)
            : this(parent, outputToUse, SettingsCollection.ForOutput(), true)
        {
        }
        internal CommandOutput(Command<TResource> parent, TResource outputToUse, SettingsCollection outputSettings)
            : this(parent, outputToUse, outputSettings, true)
        {
        }
        internal CommandOutput(Command<TResource> parent, TResource outputToUse, SettingsCollection outputSettings, bool export)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (outputSettings == null)
            {
                throw new ArgumentNullException("outputSettings");
            }
            if (outputToUse == null)
            {
                throw new ArgumentNullException("outputToUse");
            }
            if (outputSettings.Type != SettingsCollectionResourceType.Output)
            {
                throw new ArgumentException("CommandOutput only accepts output settings collections");
            }

            Parent = parent;
            Resource = outputToUse;
            Settings = outputSettings;
            IsExported = export;
        }

        public SettingsCollection Settings { get; set; }

        public Command<TResource> Parent { get; protected set; }

        public bool IsExported { get; set; }

        public TimeSpan Length
        {
            get
            {
                return TimeSpan.FromSeconds(Helpers.GetLength(Parent));
            }
        }

        public TResource GetOutput()
        {
            Resource.Length = Length;
            return Resource;
        }

        #region Internals
        internal TResource Resource { get; set; }
        #endregion
    }

    [Obsolete("CommandResource<TOutput> is obsolete, do not reference Obsolete namespace.", false)]
    public class CommandResource<TResource>
        where TResource : IResource
    {
        internal CommandResource(Command<IResource> parent, TResource resource)
            : this(parent, SettingsCollection.ForInput(), resource)
        {
        }
        internal CommandResource(Command<IResource> parent, SettingsCollection settings, TResource resource)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (settings.Type != SettingsCollectionResourceType.Input)
            {
                throw new ArgumentException("CommandResource only accepts input settings collections");
            }

            Parent = parent;
            Resource = resource;
            Settings = settings;
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// the resource input file that is part of the command.
        /// </summary>
        public TResource Resource { get; set; }

        /// <summary>
        /// the collection of settings that apply to this input
        /// </summary>
        public SettingsCollection Settings { get; set; }

        /// <summary>
        /// returns a receipt for the command resource
        /// </summary>
        /// <returns></returns>
        public CommandResourceReceipt GetReciept()
        {
            return new CommandResourceReceipt(Parent.Parent.Id, Parent.Id, Resource.Map);
        }

        #region Internals
        internal string Id { get; set; }
        internal Command<IResource> Parent { get; set; }
        #endregion
    }

    [Obsolete("CommandResourceReceipt is obsolete, do not reference Obsolete namespace.", false)]
    public class CommandResourceReceipt
    {
        internal CommandResourceReceipt(string factoryId, string commandId, string map)
        {
            Map = map;
            CommandId = commandId;
            FactoryId = factoryId;
        }

        /// <summary>
        /// referece to the command resource file
        /// </summary>
        public string Map { get; protected set; }

        /// <summary>
        /// reference to the command that holds this reference
        /// </summary>
        public string CommandId { get; protected set; }

        /// <summary>
        /// reference to the command factory that holds this reference
        /// </summary>
        public string FactoryId { get; protected set; }

        public bool Equals(CommandResourceReceipt receipt)
        {
            return Map == receipt.Map
                   && CommandId == receipt.CommandId
                   && FactoryId == receipt.FactoryId;
        }
    }
}

namespace Hudl.Ffmpeg.Settings.Obsolete.BaseTypes
{
    [Obsolete("ISetting is obsolete, do not reference Obsolete namespace.", false)]
    public interface ISetting
    {
        /// <summary>
        /// the command name for the affect
        /// </summary>
        string Type { get; }

        /// <summary>
        /// the length override function, overrided when a setting requires a length change of output calculated from the resources.
        /// </summary>
        /// <returns>Null indicates that the length difference does not apply</returns>
        TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources);

        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string ToString();
    }

    [Obsolete("BaseSetting is obsolete, do not reference Obsolete namespace.", false)]
    public abstract class BaseSetting : ISetting
    {
        protected BaseSetting(string type)
        {
            Type = type;
        }

        /// <summary>
        /// Defines the settings type, name that is given to ffmpeg
        /// </summary>
        public string Type { get; protected set; }

        /// <summary>
        /// Quick way to calculate the output length after a setting has been applied.
        /// </summary>
        public virtual TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            var totalSeconds = resources.Sum(r => r.Resource.Length.TotalSeconds);
            return totalSeconds > 0d
                       ? (TimeSpan?)TimeSpan.FromSeconds(totalSeconds)
                       : null;
        }
    }

    [Obsolete("SettingsCollection is obsolete, do not reference Obsolete namespace.", false)]
    public class SettingsCollection
    {
        internal SettingsCollection()
            : this(SettingsCollectionResourceType.Input)
        {
        }
        internal SettingsCollection(params ISetting[] settings)
            : this(SettingsCollectionResourceType.Input, settings)
        {
        }
        internal SettingsCollection(SettingsCollectionResourceType type, params ISetting[] settings)
        {
            Type = type;
            SettingsList = new List<ISetting>();
            if (settings.Length > 0)
            {
                new List<ISetting>(settings).ForEach(s => Add(s));
            }
        }

        public ReadOnlyCollection<ISetting> Items { get { return SettingsList.AsReadOnly(); } }

        public SettingsCollectionResourceType Type { get; protected set; }

        public int Count { get { return SettingsList.Count; } }

        /// <summary>
        /// returns a new settings collection instance for input collections
        /// </summary>
        public static SettingsCollection ForInput(params ISetting[] settings)
        {
            return new SettingsCollection(SettingsCollectionResourceType.Input, settings);
        }

        /// <summary>
        /// returns a new settings collection instance for output collections
        /// </summary>
        public static SettingsCollection ForOutput(params ISetting[] settings)
        {
            return new SettingsCollection(SettingsCollectionResourceType.Output, settings);
        }

        /// <summary>
        /// adds the given Setting to the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the generic type of the Setting</typeparam>
        /// <param name="setting">the Setting to be added to the SettingsCollection</param>
        public SettingsCollection Add<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }
            if (Contains(setting))
            {
                throw new ArgumentException(string.Format("The SettingsCollection already contains a type of {0}.", setting.GetType().Name), "setting");
            }
            if (!Common.Obsolete.Validate.IsSettingFor(setting, Type))
            {
                throw new ArgumentException(string.Format("The SettingsCollection is restricted only to {0} settings.", Type), "setting");
            }

            SettingsList.Add(setting);
            return this;
        }

        /// <summary>
        /// adds the given SettingsCollection range to the SettingsCollection
        /// </summary>
        /// <param name="settings">the SettingsCollection to be added  to be added to the SettingsCollection</param>
        public SettingsCollection AddRange(SettingsCollection settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.SettingsList.ForEach(s => Add(s));
            return this;
        }

        /// <summary>
        /// merges the current setting into the set based on the merge option type
        /// </summary>
        public SettingsCollection Merge<TSetting>(TSetting setting, FfmpegMergeOptionType option)
            where TSetting : ISetting
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            var alreadyContainsSetting = Contains(setting);
            if (alreadyContainsSetting)
            {
                if (option == FfmpegMergeOptionType.NewWins)
                {
                    Remove(setting);
                    Add(setting);
                }
            }
            else
            {
                Add(setting);
            }

            return this;
        }

        /// <summary>
        /// merges the current SettingsCollection into the set based on the merge option type.
        /// </summary>
        public SettingsCollection MergeRange(SettingsCollection settings, FfmpegMergeOptionType option)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (settings.Type != Type)
            {
                throw new ArgumentException(string.Format("Settings parameter must be of the same type {0} as the SettingsCollection.", Type));
            }

            settings.SettingsList.ForEach(s => Merge(s, option));
            return this;
        }

        /// <summary>
        /// determines if the settings collection already contains this setting type
        /// </summary>
        public bool Contains<TSetting>()
            where TSetting : ISetting
        {
            return SettingsList.Any(s => s is TSetting);
        }

        /// <summary>
        /// determines if the settings collection already contains this setting type
        /// </summary>
        public bool Contains<TSetting>(TSetting item)
            where TSetting : ISetting
        {
            var itemType = item.GetType();
            return SettingsList.Any(s => s.GetType().IsAssignableFrom(itemType));
        }

        /// <summary>
        /// will return the TSetting item in the settings collection list
        /// </summary>
        public TSetting Item<TSetting>()
            where TSetting : class, ISetting
        {
            return SettingsList.FirstOrDefault(s => s is TSetting) as TSetting;
        }


        /// <summary>
        /// removes the specified setting type from the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the settings type that is to be removed</typeparam>
        public SettingsCollection Remove<TSetting>()
            where TSetting : ISetting
        {
            SettingsList.RemoveAll(s => s is TSetting);
            return this;
        }

        /// <summary>
        /// removes the specified setting type from the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the settings type that is to be removed</typeparam>
        public SettingsCollection Remove<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            var settingType = setting.GetType();
            SettingsList.RemoveAll(s => s.GetType().IsAssignableFrom(settingType));
            return this;
        }

        /// <summary>
        /// removes the Setting at the given index from the SettingsCollection
        /// </summary>
        /// <param name="index">the index of the desired Setting to be removed from the SettingsCollection</param>
        public SettingsCollection RemoveAt(int index)
        {
            SettingsList.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// removes all the Setting matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public SettingsCollection RemoveAll(Predicate<ISetting> pred)
        {
            SettingsList.RemoveAll(pred);
            return this;
        }

        #region Internals
        internal List<ISetting> SettingsList { get; set; }
        #endregion
    }
}

namespace Hudl.Ffmpeg.Settings.Obsolete
{
    [Obsolete("ACodec is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IAudio))]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class ACodec : BaseSetting
    {
        private const string SettingType = "-c:a";

        public ACodec(string codec)
            : base(SettingType)
        {
            Codec = codec;
        }

        public ACodec(AudioCodecType codec)
            : this(Formats.Library(codec))
        {
        }

        public string Codec { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Codec))
            {
                throw new InvalidOperationException("Codec cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Codec);
        }
    }

    [Obsolete("AspectRatio is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class AspectRatio : BaseSetting
    {
        private const string SettingType = "-aspect";

        public AspectRatio()
            : base(SettingType)
        {
        }

        public AspectRatio(FfmpegRatio ratio)
            : this()
        {
            if (ratio == null)
            {
                throw new ArgumentNullException("ratio");
            }

            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; }

        public override string ToString()
        {
            if (Ratio == null)
            {
                throw new InvalidOperationException("Ratio cannot be null.");
            }

            return string.Concat(Type, " ", Ratio.ToRatio());
        }
    }

    [Obsolete("AudioBitRate is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IAudio))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class AudioBitRate : BaseSetting
    {
        private const string SettingType = "-b:a";

        public AudioBitRate(int rate)
            : base(SettingType)
        {
            Rate = rate;
        }

        public AudioBitRate(AudioBitRateType rate)
            : this((int) rate)
        {
        }

        public int Rate { get; set; }

        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Bit Rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate, "k");
        }
    }

    [Obsolete("BitRate is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class BitRate : BaseSetting
    {
        private const string SettingType = "-b:v";

        public BitRate(int rate)
            : base(SettingType)
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Bit Rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate, "k");
        }
    }

    [Obsolete("Dimensions is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class Dimensions : BaseSetting
    {
        private const string SettingType = "-s";

        public Dimensions()
            : base(SettingType)
        {
            Size = new Point(1, 1);
        }

        public Dimensions(ScalePresetType preset)
            : this()
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Size = scalingPresets[preset];
        }

        public Dimensions(int x, int y)
            : this()
        {
            if (x <= 0)
            {
                throw new ArgumentException("Dimensions X must be greater than zero.");
            }
            if (y <= 0)
            {
                throw new ArgumentException("Dimensions Y must be greater than zero.");
            }

            Size = new Point(x, y);
        }

        public Point Size { get; set; }

        public override string ToString()
        {
            if (Size == null)
            {
                throw new InvalidOperationException("Dimensions size cannot be null.");
            }
            if (Size.X <= 0)
            {
                throw new InvalidOperationException("Dimensions width must be greater than zero.");
            }
            if (Size.Y <= 0)
            {
                throw new InvalidOperationException("Dimensions height must be greater than zero.");
            }

            return string.Concat(Type, " ", Size.X, "x", Size.Y);
        }
    }

    [Obsolete("Duration is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)
    ]
    public class Duration : BaseSetting
    {
        private const string SettingType = "-t";

        public Duration(TimeSpan length)
            : base(SettingType)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }

        public Duration(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }

        public TimeSpan Length { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return Length;
        }

        public override string ToString()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("Duration length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("Duration length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }

    [Obsolete("FrameRate is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class FrameRate : BaseSetting
    {
        private const string SettingType = "-r";

        public FrameRate(double rate)
            : base(SettingType)
        {
            if (rate <= 0)
            {
                throw new ArgumentException("Frame rate must be greater than zero.");
            }

            Rate = rate;
        }

        public double Rate { get; set; }

        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Frame rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate);
        }
    }

    [Obsolete("Input is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IResource))]
    internal class Input : BaseSetting
    {
        private const string SettingType = "-i";

        public Input(IResource resource)
            : base(SettingType)
        {
            Resource = resource;
        }

        public IResource Resource { get; protected set; }

        public override string ToString()
        {
            if (Resource == null)
            {
                throw new InvalidOperationException("Resource cannot be empty.");
            }

            var escapedPath = Resource.FullName.Replace('\\', '/');
            return string.Concat(Type, " \"", escapedPath, "\"");
        }
    }

    [Obsolete("OverwriteOutput is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class OverwriteOutput : BaseSetting
    {
        private const string SettingType = "-y";

        public OverwriteOutput()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return Type;
        }
    }

    [Obsolete("PixelFormat is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class PixelFormat : BaseSetting
    {
        private const string SettingType = "-pix_fmt";

        public PixelFormat(string library)
            : base(SettingType)
        {
            if (string.IsNullOrWhiteSpace(library))
            {
                throw new ArgumentNullException("library");
            }

            Library = library;
        }

        public PixelFormat(PixelFormatType library)
            : this(Formats.Library(library))
        {
        }

        public string Library { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Library))
            {
                throw new InvalidOperationException("Library cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Library);
        }
    }

    [Obsolete("RemoveAudio is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class RemoveAudio : BaseSetting
    {
        private const string SettingType = "-an";

        public RemoveAudio()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return Type;
        }
    }

    [Obsolete("SeekTo is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)
    ]
    public class SeekTo : BaseSetting
    {
        private const string SettingType = "-ss";

        public SeekTo(TimeSpan length)
            : base(SettingType)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }

        public SeekTo(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }

        public TimeSpan Length { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            var overallLength = TimeSpan.FromSeconds(0);
            var baseCalculatedLength = base.LengthFromInputs(resources);
            if (baseCalculatedLength == null)
            {
                return overallLength;
            }
            return baseCalculatedLength - Length;
        }

        public override string ToString()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("SeekTo length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("SeekTo length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }

    [Obsolete("StartAt is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class StartAt : BaseSetting
    {
        private const string SettingType = "-ss";

        public StartAt(TimeSpan length)
            : base(SettingType)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }

        public StartAt(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }

        public TimeSpan Length { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            var overallLength = TimeSpan.FromSeconds(0);
            var baseCalculatedLength = base.LengthFromInputs(resources);
            if (baseCalculatedLength == null)
            {
                return overallLength;
            }
            return baseCalculatedLength - Length;
        }

        public override string ToString()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("StartAt length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("StartAt length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }

    [Obsolete("TrimShortest is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IAudio))]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class TrimShortest : BaseSetting
    {
        private const string SettingType = "-shortest";

        public TrimShortest()
            : base(SettingType)
        {
        }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return resources.Min(r => r.Resource.Length);
        }

        public override string ToString()
        {
            return Type;
        }
    }

    [Obsolete("VCodec is obsolete, do not reference Obsolete namespace.", false)]
    [AppliesToResource(Type = typeof (IVideo))]
    [Settings.BaseTypes.SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)
    ]
    public class VCodec : BaseSetting
    {
        private const string SettingType = "-c:v";

        public VCodec(string codec)
            : base(SettingType)
        {
            Codec = codec;
        }

        public VCodec(VideoCodecType codec)
            : this(Formats.Library(codec))
        {
        }

        public string Codec { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Codec))
            {
                throw new InvalidOperationException("Codec cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Codec);
        }
    }
}
#endregion

/* 
* Stage 2 Obsolete is an error state for objects that have full tested and working solutions. 
* The lifespan of an object in stage 1 obsoletion is 2 months.
*/
#region Stage 2 Obsolete

#endregion 

/* 
* Stage 3 Obsolete is to remove the code entirely
*/