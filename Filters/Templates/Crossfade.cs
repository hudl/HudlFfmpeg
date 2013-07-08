using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Filters.Templates
{
    public class Crossfade : Blend, IFilterValidator, IFilterProcessor
    {
        private const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";
        private readonly SettingsCollection _outputSettings = SettingsCollection.ForOutput(
            new OverwriteOutput(), 
            new VCodec(VideoCodecTypes.Copy));

        public Crossfade(TimeSpan duration)
        {
            Duration = duration;
            Option = BlendVideoOptionTypes.all_expr;
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

        public override TimeSpan? LengthDifference
        {
            get { return TimeSpan.FromSeconds(Duration.TotalSeconds*-1); }
        }
       
        public bool Validate(Command<IResource> command, Filterchain<IResource> filterchain, List<CommandResourceReceipt> resources)
        {
            if (resources.Count != 2)
            {
                return false;
            }
            var indexOfFirstArgsInCommand =
                command.ResourceList.FindIndex(a => a.Resource.Map == resources[0].Map);
            var indexOfSecondArgsInCommand =
                command.ResourceList.FindIndex(a => a.Resource.Map == resources[1].Map);
            return (indexOfFirstArgsInCommand + 1) == indexOfSecondArgsInCommand;
        }

        public void PrepCommands<TOutput, TResource>(Command<TOutput> command, Filterchain<TResource> filterchain) 
            where TOutput : IResource 
            where TResource : IResource
        {
            var addCommand1 = false;
            var addCommand4 = false;
            var videoTo = filterchain.Resources[1];
            var videoFrom = filterchain.Resources[0];
            var resourceTo = command.PrepResourcesFromReceipts(videoTo).FirstOrDefault();
            var resourceFrom = command.PrepResourcesFromReceipts(videoFrom).FirstOrDefault();
            if (resourceTo == null)
            {
                addCommand4 = true;
                resourceTo = command.ResourcesFromReceipts(videoTo).FirstOrDefault();
            }
            if (resourceFrom == null)
            {
                addCommand1 = true;
                resourceFrom = command.ResourcesFromReceipts(videoFrom).FirstOrDefault();
            }
            if (resourceTo == null)
            {
                throw new InvalidOperationException("To resource does not belong to the Command or Command Factory.");
            }
            if (resourceFrom == null)
            {
                throw new InvalidOperationException("From resource does not belong to the Command or Command Factory.");
            }

            var videoFromIndex = command.ResourceList.FindIndex(a => a.Resource.Map == videoFrom.Map);

            var prepCommand1 = command.PrepCommandFromReceipt(videoFrom) ??
                               new Command<IResource>(command.Parent, command.Output.Resource.Copy<TResource>(), _outputSettings);
            var prepCommand2 = new Command<IResource>(command.Parent, command.Output.Resource.Copy<TResource>(), _outputSettings);
            var prepCommand3 = command.PrepCommandFromReceipt(videoTo) ??
                               new Command<IResource>(command.Parent, command.Output.Resource.Copy<TResource>(), _outputSettings);
            var prepCommand4 = new Command<IResource>(command.Parent, command.Output.Resource.Copy<TResource>(), _outputSettings);
           
            if (addCommand1)
            {
                prepCommand1.Add(resourceFrom.Resource);
            }
            if (addCommand4)
            {
                prepCommand4.Add(resourceTo.Resource);
            }
            prepCommand2.Add(resourceFrom.Resource);
            prepCommand3.Add(resourceTo.Resource);

            var settingsCollection1 = SettingsCollection.ForInput();
            var settingsCollection2 = SettingsCollection.ForInput();
            var settingsCollection3 = SettingsCollection.ForInput();
            var settingsCollection4 = SettingsCollection.ForInput();

            var videoToLength = TimeSpan.FromSeconds(Helpers.GetLength(resourceTo));
            var videoFromLength = TimeSpan.FromSeconds(Helpers.GetLength(resourceFrom));
            var video1ADuration = videoFromLength - Duration;
            var video2BDuration = videoToLength - Duration;
            var video1BStartAt = video1ADuration;
            var startAt1A = prepCommand1.ResourceList.First().Settings.Item<StartAt>();
            var startAt2B = prepCommand3.ResourceList.First().Settings.Item<StartAt>();
            if (startAt1A != null)
            {
                video1ADuration -= startAt1A.Length;
            }
            if (startAt2B != null)
            {
                video2BDuration -= startAt2B.Length;
            }

            settingsCollection1.Add(new Duration(video1ADuration));
            settingsCollection2.Add(new Duration(Duration));
            settingsCollection2.Add(new StartAt(video1BStartAt));
            settingsCollection3.Add(new Duration(Duration));
            settingsCollection4.Add(new Duration(video2BDuration));
            settingsCollection4.Add(new StartAt(Duration));

            prepCommand1.Resources.First()
                        .Settings.MergeRange(settingsCollection1, FfmpegMergeOptionTypes.NewWins);
            prepCommand2.Resources.First()
                        .Settings.MergeRange(settingsCollection2, FfmpegMergeOptionTypes.NewWins);
            prepCommand3.Resources.First()
                        .Settings.MergeRange(settingsCollection3, FfmpegMergeOptionTypes.NewWins);
            prepCommand4.Resources.First()
                        .Settings.MergeRange(settingsCollection4, FfmpegMergeOptionTypes.NewWins);

            var receipt2A = command.Insert(videoFromIndex + 1, prepCommand3.Output.Resource);
            var receipt1B = command.Insert(videoFromIndex + 1, prepCommand2.Output.Resource);

            filterchain.SetResources(receipt1B, receipt2A);

            if (addCommand1)
            {
                command.CommandList.Add(prepCommand1);
                command.Replace(videoFrom, prepCommand1.Output.Resource);
            }
            if (addCommand4)
            {
                command.CommandList.Add(prepCommand4);
                command.Replace(videoTo, prepCommand4.Output.Resource);
            }
            command.CommandList.Add(prepCommand2);
            command.CommandList.Add(prepCommand3);

        }
    }
}
