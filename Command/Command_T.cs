using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resolution.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class Command<TOutput>
        where TOutput : IResource
    {
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

        public CommandOutput<TOutput> Output { get; protected set; }

        public TimeSpan Length { get { return TimeSpan.FromSeconds(Helpers.GetLength(this)); } }

        public IReadOnlyList<CommandResource<IResource>> Resources { get { return ResourceList.AsReadOnly();  } }

        public IReadOnlyList<Filterchain<IResource>> Filterchains { get { return Filtergraph.FilterchainList.AsReadOnly(); } }

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
                return (ResourceList.Count(r => r.Resource.Map == receipt.Map) > 0);
            }
            return false;
        }

        public bool Contains(IResource resource)
        {
            return (ResourceList.Count(r => r.Resource.Map == resource.Map) > 0);
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
            if (settings.Type != SettingsCollectionResourceTypes.Input)
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
            if (resourceDictionary.Count(kv => kv.Key.Type == SettingsCollectionResourceTypes.Output) > 0)
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
            if (settings.Type != SettingsCollectionResourceTypes.Input)
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
            if (settings.Type != SettingsCollectionResourceTypes.Input)
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

        public void SetResolution(IResolutionTemplate resolutionTemplate)
        {
            //assign the resolution template to all input resources as the first line of filterchain
            var allInputReceipts = GetResourceReceipts(); 
            allInputReceipts.ForEach(receipt => resolutionTemplate.Filterchains.ForEach(filterchain =>
                {
                    var newFilterchain = filterchain.Copy<IResource>();
                    newFilterchain.SetResources(receipt);
                    var newReceipt = new CommandResourceReceipt(Parent.Id, Id, newFilterchain.Output.Resource.Map);  
                    Filtergraph.FilterchainList.ForEach(otherFilterchain =>
                        {
                            if (otherFilterchain.ResourceList.Any(r => r.Map == receipt.Map))
                            {
                                var currentIndex = otherFilterchain.ResourceList.FindIndex(r => r.Map == receipt.Map);
                                otherFilterchain.ResourceList[currentIndex] = newReceipt;
                            }
                        });
                    Filtergraph.FilterchainList.Insert(0, newFilterchain);
                }));

            //assign and merge the output resolutio settings for the resolution saying that the new wins 
            Output.Settings.MergeRange(resolutionTemplate.OutputSettings, FfmpegMergeOptionTypes.NewWins);
        }

        /// <summary>
        /// Renders the command stream with the defualt command processor
        /// </summary>
        public CommandOutput<TOutput> Render()
        {
            return RenderWith<WinCmdProcessorReciever>();
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
                throw commandProcessor.Error;
            }

            var returnType = RenderWith(commandProcessor);

            if (!commandProcessor.Close())
            {
                throw commandProcessor.Error;
            }

            return returnType;
        }

        /// <summary>
        /// Renders the command stream with an existing command processor
        /// </summary>
        public CommandOutput<TOutput> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : ICommandProcessor
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            var commandBuilder = new CommandBuilder();
            commandBuilder.WriteCommand(this);
            
            if (!processor.Send(commandBuilder.ToString()))
            {
                throw processor.Error;
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
        #endregion

        #region Utility
        private int GetFiltersMax<TResource>(Filterchain<TResource> filterchain)
            where TResource : IResource
        {
            return filterchain.Filters.List.Min(f => f.MaxInputs);
        }
        private bool ValidateFilters<TResource>(Filterchain<TResource> filterchain, List<CommandResourceReceipt> resources) 
            where TResource: IResource, new()
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
            where TResource : IResource
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
}
