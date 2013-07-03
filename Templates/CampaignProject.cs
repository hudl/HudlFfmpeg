using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Filters.Templates;
using Hudl.Ffmpeg.Filters.Templates.Filterchain;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    /// <summary>
    /// CampaignProject is the template base for fundraising campaign videos and audio. it outputs the following 
    ///   - Campaign m4a (AAC Interview audio) 
    ///   - Campaign Jpg (campaign image) 
    ///   - Campaign mp4 (480p resolution w/audio)
    ///   - Campaign mp4 (240p resolution w/audio)
    /// </summary>
    public class CampaignProject 
    {
        private const string BlendExpression = "A*(6/10)+B*(1-(6/10))";
        private readonly M4A _musicBackground = new M4A("C:/Source/Campaigns/assets/music.m4a", TimeSpan.FromSeconds(212)); 
        private readonly Mp3 _musicSilence = new Mp3("C:/Source/Campaigns/assets/silence.mp3", TimeSpan.FromSeconds(1));
        private readonly Mp4 _videoFilmGrain = new Mp4("C:/Source/Campaigns/assets/overlay.mp4", TimeSpan.FromSeconds(212));
        private readonly Png _imageVignette = new Png("C:/Source/Campaigns/assets/vignete.png"); 

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

        public List<IResource> Render()
        {
            SetupTemplate();

            return Factory.Render();
        }

        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            SetupTemplate();

            return Factory.RenderWith<TProcessor>();
        }

        public List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : ICommandProcessor, new()
        {
            SetupTemplate();

            return Factory.RenderWith(processor);
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
            #region ...
            var campaignM4A = Factory.OutputTo<M4A>();
            var campaignM4AResource1 = campaignM4A.Add(_musicSilence);
            var campaignM4AResource2 = campaignM4A.Add(_musicBackground);
            var campaignM4AReceipts = AudioList.Select(campaignM4A.Add).ToList();
            CommandResourceReceipt lastM4AReceipt = null;

            //FILTERS/SETTINGS
            var filterchainM4A1 = Filterchain.FilterTo<M4A>(
                new Volume(.2m)
            );
            var filterchainM4A2 = Filterchain.FilterTo<M4A>(
                new Concat(1, 0)
            );
            var filterchainM4A3 = Filterchain.FilterTo<M4A>(
                new Volume(1.5m)
            );
            var filterchainM4A4 = Filterchain.FilterTo<M4A>(
                new AMix(DurationTypes.First)
            );
            var filterchainM4A5 = Filterchain.FilterTo<M4A>(
                new AFade(FadeTransitionTypes.Out, 2)
            );

            //FILTER APPLICATION
            campaignM4A.ApplyFilter(filterchainM4A1, campaignM4AResource2); 
            
            campaignM4AReceipts.ForEach(receipt =>
                {
                    lastM4AReceipt = lastM4AReceipt == null ?
                        campaignM4A.ApplyFilter(filterchainM4A2, campaignM4AResource1, receipt) :
                        campaignM4A.ApplyFilter(filterchainM4A2, lastM4AReceipt, campaignM4AResource1, receipt);
                });

            lastM4AReceipt = campaignM4A.ApplyFilter(filterchainM4A3, lastM4AReceipt);

            lastM4AReceipt = campaignM4A.ApplyFilter(filterchainM4A4, campaignM4AResource2, lastM4AReceipt);

            campaignM4A.ApplyFilter(filterchainM4A5, lastM4AReceipt);
            #endregion

            // **********************************
            // Campaign mp4 (480p resolution w/o audio)
            // **********************************
            #region ...
            var campaign480Mp4 = Factory.OutputTo<Mp4>();
            var campaign480Mp4Resource1 = campaignM4A.Add(_imageVignette);
            var campaign480Mp4Resource2 = campaignM4A.Add(_videoFilmGrain);
            var campaign480Mp4Receipts = VideoList.Select(campaign480Mp4.Add).ToList();
            CommandResourceReceipt last480Mp4Receipt = null;

            //FILTERS/SETTINGS
            var filterchain480Mp41 = new Resolution480P<Mp4>();
            var filterchain480Mp42 = Filterchain.FilterTo<Mp4>(
                new Crossfade(TimeSpan.FromSeconds(1))
            );
            var filterchain480Mp43 = Filterchain.FilterTo<Mp4>(
                new Concat()
            );
            var filterchain480Mp44 = Filterchain.FilterTo<Mp4>(
                new Overlay()
            );
            var filterchain480Mp45 = Filterchain.FilterTo<Mp4>(
                new Blend(BlendExpression)
            ); 
            var filterchain480Mp46 = Filterchain.FilterTo<Mp4>(
                new ColorBalance(_shadows, _midtones, _highlights),
                new Fade(FadeTransitionTypes.Out, 2)
            );
            var outputSettings480Mp4 = SettingsCollection.ForOutput(
                new RemoveAudio(),
                new TrimShortest(),
                new OverwriteOutput(),
                new BitRate(3000),
                new FrameRate(29.97),
                new Dimensions(ScalePresetTypes.Hd480),
                new PixelFormat(PixelFormatTypes.Yuv420P),
                new VCodec(VideoCodecTypes.Libx264)
            );

            //FILTER APPLICATION 
            campaign480Mp4.ApplyFilterToEach(filterchain480Mp41);

            campaign480Mp4Receipts.ForEach(receipt =>
                {
                    if (last480Mp4Receipt == null)
                    {
                        last480Mp4Receipt = receipt;
                        return;
                    }

                    campaign480Mp4.ApplyFilter(filterchain480Mp42, last480Mp4Receipt, receipt);
                });

            last480Mp4Receipt = campaign480Mp4.ApplyFilter(filterchain480Mp43);

            last480Mp4Receipt = campaign480Mp4.ApplyFilter(filterchain480Mp44, last480Mp4Receipt, campaign480Mp4Resource1);

            last480Mp4Receipt = campaign480Mp4.ApplyFilter(filterchain480Mp45, last480Mp4Receipt, campaign480Mp4Resource2);

            campaign480Mp4.ApplyFilter(filterchain480Mp46, last480Mp4Receipt);

            campaign480Mp4.Output.Settings = outputSettings480Mp4; 
            #endregion

            // **********************************
            // Campaign mp4 (240p resolution w/o audio)
            // **********************************
            #region ...
            var campaign240Mp4 = Factory.OutputTo<Mp4>(false);
            campaign240Mp4.Add(campaign480Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var filterchain240Mp41 = new Resolution240P<Mp4>();
            var outputSettings240Mp4 = SettingsCollection.ForOutput(
                new RemoveAudio(),
                new TrimShortest(),
                new OverwriteOutput(),
                new BitRate(3000),
                new FrameRate(29.97),
                new Dimensions(ScalePresetTypes.Hd480),
                new PixelFormat(PixelFormatTypes.Yuv420P),
                new VCodec(VideoCodecTypes.Libx264)
            );

            //FILTER APPLICATION 
            campaign240Mp4.ApplyFilter(filterchain240Mp41);

            campaign240Mp4.Output.Settings = outputSettings240Mp4; 
            #endregion

            // **********************************
            // Campaign mp4 (480p resolution w/audio)
            // **********************************
            #region ...
            var campaign480Mp4WAudio = Factory.OutputTo<Mp4>();
            campaign480Mp4WAudio.Add(campaignM4A.Output.Resource);
            campaign480Mp4WAudio.Add(campaign480Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var outputSettings480Mp4WAudio = SettingsCollection.ForOutput(
                new TrimShortest(),
                new OverwriteOutput(),
                new ACodec(AudioCodecTypes.Copy),
                new VCodec(VideoCodecTypes.Copy)
            );

            //FILTER APPLICATION 
            campaign480Mp4WAudio.Output.Settings = outputSettings480Mp4WAudio; 
            #endregion

            // **********************************
            // Campaign mp4 (240p resolution w/audio)
            // **********************************
            #region ...
            var campaign240Mp4WAudio = Factory.OutputTo<Mp4>();
            campaign240Mp4WAudio.Add(campaignM4A.Output.Resource);
            campaign240Mp4WAudio.Add(campaign240Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var outputSettings240Mp4WAudio = SettingsCollection.ForOutput(
                new TrimShortest(),
                new OverwriteOutput(),
                new ACodec(AudioCodecTypes.Copy),
                new VCodec(VideoCodecTypes.Copy)
            );

            //FILTER APPLICATION 
            campaign240Mp4WAudio.Output.Settings = outputSettings240Mp4WAudio;
            #endregion

            // **********************************
            // Campaign Jpg (240p resolution w/audio)
            // **********************************
            #region ...
            var campaignJpg = Factory.OutputTo<Jpg>();
            campaignJpg.Add(campaign480Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var outputSettingsJpg = SettingsCollection.ForOutput(
                new OverwriteOutput(),
                new FrameRate(1),
                new Duration(1),
                new StartAt(TimeSpan.FromSeconds(5))
            );

            //FILTER APPLICATION 
            campaignJpg.Output.Settings = outputSettingsJpg;
            #endregion
        }


        #region Internals
        internal protected List<IVideo> VideoList { get; protected set; }
        internal protected List<IAudio> AudioList { get; protected set; }
        internal protected CommandFactory Factory { get; protected set; }
        #endregion 
    }
}
