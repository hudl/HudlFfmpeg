using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class Command<TOutput>
        where TOutput : IResource, new()
    {
        internal Command(CommandFactory parent)
        {
            Id = Guid.NewGuid().ToString();
            Parent = parent;
            Filtergraph = new Filtergraph();
            CommandList = new List<Command<IResource>>();
            ResourceList = new List<CommandResource<IResource>>();
        }

        public CommandResource<TOutput> Output { get; protected set; }

        public TimeSpan Length { get { Helpers.GetLength(this); } }

        public IReadOnlyList<CommandResource<IResource>> Resources { get { return ResourceList.AsReadOnly();  } }

        public CommandResourceReceipt Add<TResource>(string path) 
            where TResource : IResource, new()
        {
            return Add<TResource>(SettingsCollection.ForInput(), path);
        }

        public CommandResourceReceipt Add<TResource>(string path, TimeSpan length) 
            where TResource : IResource, new()
        {
            return Add<TResource>(SettingsCollection.ForInput(), path, length);
        }

        public CommandResourceReceipt Add<TResource>(SettingsCollection settings, string path)
            where TResource : IResource, new()
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Resource path cannot be null or empty.", "path");
            }

            return Add(settings,
                       new TResource
                       {
                           Path = path
                       });
        }

        public CommandResourceReceipt Add<TResource>(SettingsCollection settings, string path, TimeSpan length)
            where TResource : IResource, new()
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Resource path cannot be null or empty.", "path");
            }
            if (length == null)
            {
                throw new ArgumentException("Resource length cannot be null or empty.", "length");
            }

            return Add(settings,
                       new TResource
                           {
                               Path = path,
                               Length = length
                           });
        }

        public CommandResourceReceipt Add(IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return Add(SettingsCollection.ForInput(SettingsCollectionResourceTypes.Input), resource);
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

            return resourceDictionary.Select(r => Add(r.Key, r.Value)).ToList();
        }

        public CommandResourceReceipt GetReceipt(Func<IResource, bool> predicate)
        {
            return GetReceipts(predicate).FirstOrDefault();
        }

        public List<CommandResourceReceipt> GetReceipts(Func<IResource, bool> predicate)
        {
            return ResourceList.Where(predicate)
                               .Select(r => r.GetReciept())
                               .ToList();
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
                resourceList = ResourceList.Select(r => new CommandResourceReceipt(r.Resource.Map)).ToList();
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

            return ApplyFilter(new Filterchain<TResource>(filter), resources);
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
                resourceList = ResourceList.Select(r => new CommandResourceReceipt(r.Resource.Map)).ToList();
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
            if (!ValidateFiltersMax(filterchain))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, exceeds maximum calculated allowable resources.");
            }
            if (!ValidateFilters(filterchain))
            {
                throw new InvalidOperationException(
                    "Filterchain is invalid, failed to comply with child filter requirements.");
            }

            //create a new empty and blank filterchain
            var filterchainCopy = filterchain.Copy();
            ProcessFilters(filterchainCopy);
            filterchainCopy.SetResources(resources);
            Filtergraph.Add(filterchainCopy);
            return new CommandResourceReceipt(Parent.Id, Id, filterchain.Output.Map);    
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

            return ApplyFilterToEach(new Filterchain<TResource>(filter), resources);
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
                resourceList = ResourceList.Select(r => new CommandResourceReceipt(r.Resource.Map)).ToList();
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

        public TOutput Render()
        {
            return RenderWith<BatchCommandProcessorReciever>();
        }

        public TOutput RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            
        }

        #region Internals
        internal string Id { get; set; }
        internal CommandFactory Parent { get; set; }
        internal Filtergraph Filtergraph { get; set; }
        internal List<Command<IResource>> CommandList { get; set; }
        internal List<CommandResource<IResource>> ResourceList { get; set; }
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

            var receiptMaps = receipts.Select(d => d.Map);
            return ResourceList.Where(r => receiptMaps.Contains(r.Resource.Map)).ToList();
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

            return CommandList.Where(c => receipt.CommandId == c.Id);
        }
        #endregion

        #region Utility
        private int GetFiltersMax<TResource>(Filterchain<TResource> filterchain)
            where TResource : IResource
        {
            return filterchain.Filters.List.Min(f => f.MaxInputs);
        }
        private bool ValidateFilters<TResource>(Filterchain<TResource> filterchain) 
            where TResource: IResource, new()
        {
            return filterchain.Filters.List.TrueForAll(f =>
                {
                    if (!(f is IFilterValidator))
                    {
                        return true;
                    }
                    return (f as IFilterValidator).Validate(this, filterchain);
                });
        }
        private bool ValidateFiltersMax<TResource>(Filterchain<TResource> filterchain)
            where TResource : IResource
        {
            var maximumAllowedMinimum = GetFiltersMax(filterchain);
            return (maximumAllowedMinimum == 1 && filterchain.Resources.Count > maximumAllowedMinimum);
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
                    var prepatoryCommands = (filter as IFilterProcessor).GetCommands(this, filterchain);
                    if (prepatoryCommands == null) return;
                    CommandList.AddRange(prepatoryCommands);
                });
        }
        private void ProcessFilterchain<TResource>(Filterchain<TResource> filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var maximumAllowedMinimum = GetFiltersMax(filterchain);
            
            
        }
        #endregion

    }
}
