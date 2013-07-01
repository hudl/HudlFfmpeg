using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    /// <summary>
    /// CampaignProject is the template base for fundraising campaign videos and audio. it outputs the following 
    ///   - Campaign m4a (AAC Interview audio) 
    ///   - Campaign png (campaign image) 
    ///   - Campaign mp4 (480p resolution w/o audio)
    ///   - Campaign mp4 (240p resolution w/o audio)
    ///   - Campaign mp4 (480p resolution w/audio)
    ///   - Campaign mp4 (240p resolution w/audio)
    /// </summary>
    public class CampaignProject 
    {
        private const string BlendExpression = "A*(6/10)+B*(1-(6/10))";
        private M4A MusicPathBackground = new M4A("C:/Source/Campaigns/assets/music.m4a", TimeSpan.FromSeconds(212)); 
        private Mp3 MusicPathSilence = new Mp3("C:/Source/Campaigns/assets/silence.mp3", TimeSpan.FromSeconds(1));

        private FfmpegScaleRgb _shadows = new FfmpegScaleRgb
        {
            Blue = new FfmpegScale(.2M)
        };
        private FfmpegScaleRgb _midtones = new FfmpegScaleRgb
        {
            Red = new FfmpegScale(.1M),
            Blue = new FfmpegScale(-.1M)
        };
        private FfmpegScaleRgb _highlights = new FfmpegScaleRgb
        {
            Red = new FfmpegScale(.1M),
            Blue = new FfmpegScale(-.2M)
        };
       
        public CampaignProject()
        {
            Factory = new CommandFactory();        
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

        public object Render()
        {
            return RenderWith<BatchCommandProcessorReciever>();
        }
        
        public object RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
        }

        private void SetupTemplate()
        {
            if (VideoList.Count == 0)
            {
                throw new InvalidOperationException("Cannot create a campaign project with an empty video list.");
            }
            if (AudioList.Count == 0)
            {
                throw new InvalidOperationException("Cannot create a campaign project with an empty audio list.");
            }
            
            // **********************************
            // Campaign m4a (AAC Interview audio) 
            // **********************************
            var campaignM4A = Factory.OutputAs<M4A>();
            var campaignResource1 = campaignM4A.Add(MusicPathSilence);
            var campaignResource2 = campaignM4A.Add(MusicPathBackground);
            var campaignM4AReceipts = AudioList.Select(campaignM4A.Add).ToList();
            CommandResourceReceipt lastM4AReceipt = null;

            //FILTERS/SETTINGS
            #region ...
            var filterchain1 = Filterchain.FilterTo<M4A>(
                new Volume(.2m)
            );
            var filterchain2 = Filterchain.FilterTo<M4A>(
                new Concat(1, 0)
            );
            var filterchain3 = Filterchain.FilterTo<M4A>(
                new Volume(1.5m)
            );
            var filterchain4 = Filterchain.FilterTo<M4A>(
                new AMix(DurationTypes.First)
            );
            var filterchain5 = Filterchain.FilterTo<M4A>(
                new AFade(FadeTransitionTypes.Out, 2)
            );
            #endregion

            campaignM4A.ApplyFilter(filterchain1, campaignResource2); 
            
            campaignM4AReceipts.ForEach(receipt =>
                {
                    lastM4AReceipt = lastM4AReceipt == null ? 
                        campaignM4A.ApplyFilter(filterchain2, campaignResource1, receipt) : 
                        campaignM4A.ApplyFilter(filterchain2, lastM4AReceipt, campaignResource1, receipt);
                });

            lastM4AReceipt = campaignM4A.ApplyFilter(filterchain3, lastM4AReceipt); 

            lastM4AReceipt = campaignM4A.ApplyFilter(filterchain4, lastM4AReceipt )

        }


        #region Internals
        internal protected List<IVideo> VideoList { get; protected set; }
        internal protected List<IAudio> AudioList { get; protected set; }
        internal protected CommandFactory Factory { get; protected set; }
        #endregion 
    }
}
