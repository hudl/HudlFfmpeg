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
       
        public bool Validate(Command<IResource> command, Filterchain<IResource> filterchain)
        {
            if (filterchain.Resources.Count != 2)
            {
                return false;
            }
            var indexOfFirstArgsInCommand =
                command.ResourceList.FindIndex(a => a.Resource.Map == filterchain.Resources[0].Map);
            var indexOfSecondArgsInCommand =
                command.ResourceList.FindIndex(a => a.Resource.Map == filterchain.Resources[0].Map);
            return (indexOfFirstArgsInCommand + 1) == indexOfSecondArgsInCommand;
        }

        public void PrepCommands<TOutput, TResource>(Command<TOutput> command, Filterchain<TResource> filterchain) 
            where TOutput : IResource
            where TResource : IResource
        {
            var prepatoryCommandMiddle = new Command<IResource>(command.Parent);
            var prepatoryCommandBeggining = new Command<IResource>(command.Parent);
            var receiptVideoTo = filterchain.Resources[1];
            var receiptVideoFrom = filterchain.Resources[0];
            //var begginingReceipt = receiptVideoFrom;
            //var middleReceipt = receiptVideoFrom;
            var receiptVideoToIndex = command.ResourceList.FindIndex(c => c.Resource.Map == receiptVideoTo.Map);
            var receiptVideoFromIndex = command.ResourceList.FindIndex(c => c.Resource.Map == receiptVideoFrom.Map);
            var resourceTo = command.PrepResourcesFromReceipts(receiptVideoTo).FirstOrDefault();
            var resourceFrom = command.PrepResourcesFromReceipts(receiptVideoFrom).FirstOrDefault();
            if (resourceTo == null)
            {
                resourceTo = command.ResourcesFromReceipts(receiptVideoTo).FirstOrDefault();
            }
            else
            {
                prepatoryCommandMiddle = command.PrepCommandFromReceipt(receiptVideoTo);
            }

            if (resourceFrom == null)
            {
                resourceFrom = command.ResourcesFromReceipts(receiptVideoFrom).FirstOrDefault();
            }
            else
            {
                prepatoryCommandBeggining = command.PrepCommandFromReceipt(receiptVideoFrom);
            }
            
            if (resourceTo == null)
            {
                throw new InvalidOperationException("To resource does not belong to the Command or Command Factory.");
            }
            if (resourceFrom == null)
            {
                throw new InvalidOperationException("From resource does not belong to the Command or Command Factory.");
            }

            var currentFromVideoLength = TimeSpan.FromSeconds(Helpers.GetLength(resourceFrom));
            var videoDuration1 = currentFromVideoLength - Duration;
            var videoDuration2 = currentFromVideoLength - Duration - TimeSpan.FromSeconds(1);
            
            if (prepatoryCommandBeggining == null)
            {
                prepatoryCommandBeggining = new Command<IResource>(command.Parent);
                prepatoryCommandBeggining.Add(resourceFrom.Resource);
            }
            if (prepatoryCommandMiddle == null)
            {
                prepatoryCommandMiddle = new Command<IResource>(command.Parent);
                prepatoryCommandMiddle.Add(resourceTo.Resource);
            }

            var endSettingsCollection = SettingsCollection.ForInput(); 
            var begginingSettingsCollection = SettingsCollection.ForInput(new OverwriteOutput(),
                                                                          new VCodec(VideoCodecTypes.Copy));
            var middleSettingsCollection = SettingsCollection.ForInput(new OverwriteOutput(),
                                                                       new VCodec(VideoCodecTypes.Copy));

            if (receiptVideoFromIndex == 0)
            {
                begginingSettingsCollection.Add(new Duration(videoDuration1));
                middleSettingsCollection.AddRange(SettingsCollection.ForInput(
                    new StartAt(videoDuration1), 
                    new Duration(Duration)
                ));
            }
            else if (receiptVideoToIndex == (command.ResourceList.Count - 1))
            {
                begginingSettingsCollection.Add(new Duration(Duration));
                middleSettingsCollection.AddRange(SettingsCollection.ForInput(
                    new StartAt(Duration), 
                    new Duration(videoDuration1)
                ));
            }
            else
            {
                begginingSettingsCollection.Add(new Duration(Duration));
                middleSettingsCollection.AddRange(SettingsCollection.ForInput(
                    new StartAt(Duration), 
                    new Duration(videoDuration2)
                ));
                endSettingsCollection.AddRange(SettingsCollection.ForInput(
                    new OverwriteOutput(),
                    new VCodec(VideoCodecTypes.Copy),
                    new StartAt(videoDuration1),
                    new Duration(Duration)
                ));
            }

            prepatoryCommandBeggining.Resources.First()
                                     .Settings.Merge(begginingSettingsCollection, FfmpegMergeOptionTypes.NewWins);
            prepatoryCommandMiddle.Resources.First()
                                  .Settings.Merge(middleSettingsCollection, FfmpegMergeOptionTypes.NewWins);

            if (endSettingsCollection.Items.Count > 0)
            {
                var prepatoryCommandEnd = new Command<IResource>(command.Parent);
                prepatoryCommandEnd.Add(endSettingsCollection, resourceFrom.Resource);
                command.CommandList.Add(prepatoryCommandEnd);
            }
        }
    }
}
