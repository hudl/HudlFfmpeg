using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates;

namespace Hudl.Ffmpeg.Filters.Templates
{
    public class Crossfade : Blend, IFilterProcessor
    {
        private const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";

        public Crossfade(TimeSpan duration, Filterchain resolutionFilterchain)
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

        private Filterchain ResolutionFilterchain { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource> resources)
        {
            return Duration;
        }

        //TODO: CB -> it would be relatively easy to reconfigure this routine to utilize splits in order to do this more effeciently
        public void PrepCommands(FfmpegCommand command, Filterchain filterchain)
        {
            //verify that we have a resolution filterchain 
            if (ResolutionFilterchain == null)
            {
                throw new InvalidOperationException("A resolution is required for the cross fade command, because of the Blend filter.");
            }

            double resourceToLength;
            double resourceFromLength;
            Filterchain filterchainCutupTransitionTo = null;
            Filterchain filterchainCutupTransitionFrom = null;

            var receiptTo = filterchain.Resources[1];
            var receiptFrom = filterchain.Resources[1];
            var resourceTo = command.ResourcesFromReceipts(receiptTo).First();
            var resourceFrom = command.ResourcesFromReceipts(receiptFrom).First();
            var filterchainCutupBodyTo = command.FilterchainFromReceipt(receiptTo);
            var filterchainCutupBodyFrom = command.FilterchainFromReceipt(receiptFrom);

            //validate that the resources in fact come from the appropriate command
            if (resourceTo == null)
            {
                throw new InvalidOperationException("To resource does not belong to the Command or Command Factory.");
            } 
            if (resourceFrom == null)
            {
                throw new InvalidOperationException("From resource does not belong to the Command or Command Factory.");
            } 

            //get the filterchain body cutup
            if (receiptTo.Type == CommandReceiptType.Stream)
            {
                resourceToLength = filterchainCutupBodyTo.Outputs(command).First().Length.TotalSeconds;
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

            if (receiptFrom.Type == CommandReceiptType.Stream)
            {
                resourceFromLength = filterchainCutupBodyFrom.Outputs(command).First().Length.TotalSeconds;
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
                var filterchainCutupBodyToResource = resourceTo.Resource.Copy<IResource>();
                filterchainCutupBodyTo = VideoCutTo.Create(filterchainCutupBodyToResource, durationLength, resourceToLength);
            }
            else
            {
                var trimFilter = filterchainCutupBodyTo.Filters.Get<Trim>();
                trimFilter.End -= Duration.TotalSeconds;
                trimFilter.Duration = trimFilter.End - trimFilter.Start;
                filterchainCutupBodyTo.Filters.Merge(trimFilter, FfmpegMergeOptionType.NewWins); 
            }
            var filterchainCutupTransitionToResource = resourceTo.Resource.Copy<IResource>();
            filterchainCutupTransitionTo = VideoCutTo.Create(filterchainCutupTransitionToResource, 0D, durationLength);

            //an existing trim cannot be located, so a new one is required
            if (filterchainCutupBodyFrom == null)
            {
                var filterchainCutupBodyFromResource = resourceFrom.Resource.Copy<IResource>();
                filterchainCutupBodyFrom = VideoCutTo.Create(filterchainCutupBodyFromResource, 0D, durationFromEndLength);
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
            var filterchainCutupTransitionFromResource = resourceFrom.Resource.Copy<IResource>();
            filterchainCutupTransitionFrom = VideoCutTo.Create(filterchainCutupTransitionFromResource, durationFromEndLength, resourceFromLength);

            //reset the filtergraph outputs
            if (receiptTo.Type == CommandReceiptType.Input)
            {
                var newInputReceipt = command.RegenerateResourceMap(filterchain.Resources[1]);
                filterchainCutupBodyTo.SetResources(newInputReceipt);
                filterchainCutupTransitionTo.SetResources(newInputReceipt);
            }
            else
            {
                filterchainCutupTransitionTo.SetResources(filterchainCutupBodyTo.Resources.First());
            }

            if (receiptFrom.Type == CommandReceiptType.Input)
            {
                var newInputReceipt = command.RegenerateResourceMap(filterchain.Resources[0]);
                filterchainCutupBodyFrom.SetResources(newInputReceipt);
                filterchainCutupTransitionFrom.SetResources(newInputReceipt);
            }
            else
            {
                filterchainCutupTransitionFrom.SetResources(filterchainCutupBodyFrom.Resources.First());
            }

            filterchainCutupBodyFrom.OutputList.First().Resource.Map = filterchain.Resources[0].Map;
            filterchainCutupBodyTo.OutputList.First().Resource.Map = filterchain.Resources[1].Map;

            command.Objects.Filtergraph.Merge(filterchainCutupBodyFrom, FfmpegMergeOptionType.NewWins);
            command.Objects.Filtergraph.Add(filterchainCutupTransitionFrom);
            command.Objects.Filtergraph.Add(filterchainCutupTransitionTo);
            command.Objects.Filtergraph.Merge(filterchainCutupBodyTo, FfmpegMergeOptionType.NewWins);

            var transitionToReceipt = CommandReceipt.CreateFromStream(command.Owner.Id, command.Id, filterchainCutupTransitionTo.OutputList.First().Resource.Map);
            var transitionFromReceipt = CommandReceipt.CreateFromStream(command.Owner.Id, command.Id, filterchainCutupTransitionFrom.OutputList.First().Resource.Map);
            var filterchainCopyTo = ResolutionFilterchain.Copy();
            var filterchainCopyFrom = ResolutionFilterchain.Copy();
            filterchainCopyTo.SetResources(transitionToReceipt);
            filterchainCopyFrom.SetResources(transitionFromReceipt);
            command.FilterchainManager.Add(filterchainCopyTo);
            command.FilterchainManager.Add(filterchainCopyFrom);

            //assign new receipts to the input filterchain
            var toReceipt = CommandReceipt.CreateFromStream(command.Owner.Id, command.Id, filterchainCopyTo.OutputList.First().Resource.Map);
            var fromReceipt = CommandReceipt.CreateFromStream(command.Owner.Id, command.Id, filterchainCopyFrom.OutputList.First().Resource.Map);
            filterchain.SetResources(toReceipt, fromReceipt);
        }
    }
}
