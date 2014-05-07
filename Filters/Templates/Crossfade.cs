using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Templates;

namespace Hudl.Ffmpeg.Filters.Templates
{
    //TODO: CB: cross fade with new splits will be off limits until needed under the new workflow, logic to calculate the time of the current stream is still required.
    internal class Crossfade : Blend, IFilterProcessor
    {
        private const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";

        public Crossfade(TimeSpan duration)
        {
            Duration = duration;
            Option = BlendVideoOptionType.all_expr;
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

        public override TimeSpan? LengthFromInputs(List<CommandResource> resources)
        {
            return Duration;
        }

        public void PrepCommands(FfmpegCommand command, Filterchain filterchain)
        {
            var streamTo = filterchain.ReceiptList[1];
            var streamFrom = filterchain.ReceiptList[0]; 

            //grab the current length of the receipt specified 
            var streamToLength = Helpers.GetLength(command, streamTo);
            var streamFromLength = Helpers.GetLength(command, streamFrom);

            //split the input streams into two outputs, using the output type of the input chain 
            var splitStreamTo = filterchain.CloneAndEmpty();
            var splitStreamFrom = filterchain.CloneAndEmpty();
            splitStreamTo.Filters.Add(new Split(2));
            splitStreamFrom.Filters.Add(new Split(2)); 
            var stageTo = command.WithStreams(streamTo)
                                 .Filter(splitStreamTo);
            var stageFrom = command.WithStreams(streamFrom)
                                   .Filter(splitStreamFrom); 

            //now that we have the stage split, it is time to perform the necessary trims 
            var filterchainTo1 = filterchain.CloneAndEmpty();
            var filterchainTo2 = filterchain.CloneAndEmpty();
            var filterchainFrom1 = filterchain.CloneAndEmpty();
            var filterchainFrom2 = filterchain.CloneAndEmpty();
            filterchainFrom1.Filters.Add(new Trim(0d, streamFromLength - Duration.TotalSeconds, VideoUnitType.Seconds));
            filterchainFrom2.Filters.Add(new Trim(streamFromLength - Duration.TotalSeconds, streamFromLength, VideoUnitType.Seconds));
            filterchainTo1.Filters.Add(new Trim(0d, Duration.TotalSeconds, VideoUnitType.Seconds));
            filterchainTo2.Filters.Add(new Trim(Duration.TotalSeconds, streamToLength, VideoUnitType.Seconds));
            var stage1 = command.WithStreams(stageFrom.Receipts.ElementAt(0))
                                .Filter(filterchainFrom1);
            var stage1Filterchain = command.Objects.Filtergraph.FilterchainList.Last();
            var stage2 = command.WithStreams(stageFrom.Receipts.ElementAt(1))
                                .Filter(filterchainFrom2);
            var stage3 = command.WithStreams(stageTo.Receipts.ElementAt(0))
                                .Filter(filterchainTo1);
            var stage4 = command.WithStreams(stageTo.Receipts.ElementAt(1))
                                .Filter(filterchainTo2);
            var stage4Filterchain = command.Objects.Filtergraph.FilterchainList.Last();

            //finally its time to set the resources and change the maps
            filterchain.SetResources(stage2.Receipts.First(), stage3.Receipts.First());
            stage4Filterchain.OutputList.First().Resource.Map = streamTo.Map;
            stage1Filterchain.OutputList.First().Resource.Map = streamFrom.Map;
            command.RegenerateResourceMap(streamTo);
            command.RegenerateResourceMap(streamFrom);
        }
    }
}
