using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class Command<TOutput>
        where TOutput : IResource, new()
    {
        internal Command()
        {
            Filtergraph = new Filtergraph();
            CommandList = new List<Command<IResource>>();
            ResourceList = new List<CommandResource<IResource>>();
        }

        public CommandResource<TOutput> Output { get; protected set; }

        public IReadOnlyList<CommandResource<IResource>> Resources { get { return ResourceList.AsReadOnly();  } }

        public CommandResourceReceipt Add<TResource>(string path) 
            where TResource : IResource, new()
        {
            return Add<TResource>(new SettingsCollection(), path);
        }

        public CommandResourceReceipt Add<TResource>(string path, TimeSpan length) 
            where TResource : IResource, new()
        {
            return Add<TResource>(new SettingsCollection(), path, length);
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

            return Add(new SettingsCollection(), resource);
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

            var commandResource = new CommandResource<IResource>(settings, resource);
            ResourceList.Add(commandResource);
            return new CommandResourceReceipt(resource.Map);
        }

        public CommandResourceReceipt GetReceipt(Func<IResource, bool> predicate)
        {
            return GetReceipts(predicate)
                    .FirstOrDefault();
        }

        public List<CommandResourceReceipt> GetReceipts(Func<IResource, bool> predicate)
        {
            return ResourceList.Select(r => r.Resource)
                                 .ToList()
                                 .Where(predicate)
                                 .Select(r => new CommandResourceReceipt(r.Map))
                                 .ToList();
        }

        public Command<TOutput> ApplySettings<TSetting>(TSetting setting, params CommandResourceReceipt[] resources)
            where TSetting : ISetting
        {
            if (setting == null) 
            {
                throw new ArgumentNullException("setting");
            }

            return ApplySettings(new SettingsCollection(setting), resources);
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

            var newFilterchain = filterchain.Copy();
            newFilterchain.SetResources(resources);
            Filtergraph.Add(newFilterchain);
            return new CommandResourceReceipt(filterchain.Output.Map);    
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

            var receiptList = new List<CommandResourceReceipt>();
            resources.ForEach(resource =>
                {
                    var newFilterchain = filterchain.Copy();
                    newFilterchain.SetResources(resource);
                    Filtergraph.Add(filterchain);
                    receiptList.Add(new CommandResourceReceipt(filterchain.Output.Map));
                });

            return receiptList;
        }

        #region Internals
        internal Filtergraph Filtergraph { get; set; }
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
        #endregion
    }
}
