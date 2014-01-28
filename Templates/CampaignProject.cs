using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Filters.Obsolete;
using Hudl.Ffmpeg.Filters.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Filters.Obsolete.Templates;
using Hudl.Ffmpeg.Command.Obsolete;

namespace Hudl.Ffmpeg.Templates
{
    /// <summary>
    /// CampaignProject is the template base for fundraising campaign videos and audio. it outputs the following 
    ///   - Campaign m4a (AAC Interview audio) 
    ///   - Campaign Jpg (campaign image) 
    ///   - Campaign mp4 (480p resolution w/audio)
    ///   - Campaign mp4 (240p resolution w/audio)
    /// </summary>
    public class CampaignProject : BaseCommandFactoryTemplate
    {
        private const string BlendExpression = "A*(6/10)+B*(1-(6/10))";
        private readonly M4A _musicBackground; 
        private readonly Mp3 _musicSilence;
        private readonly Mp4 _videoFilmGrain;
        private readonly Png _imageVignette;

        private readonly FfmpegScaleRgb _shadows = new FfmpegScaleRgb
        {
            Blue = new FfmpegScale(.2M)
        };
        private readonly FfmpegScaleRgb _midtones = new FfmpegScaleRgb
        {
            Red = new FfmpegScale(.1M),
            Blue = new FfmpegScale(-.1M)
        };
        private readonly FfmpegScaleRgb _highlights = new FfmpegScaleRgb
        {
            Red = new FfmpegScale(.1M),
            Blue = new FfmpegScale(-.2M)
        };
       
        public CampaignProject(CommandConfiguration configuration, M4A assetMusic, Mp3 assetSilence, Mp4 assetVideo, Png assetImage)
            : base(configuration)
        {
            _musicBackground = assetMusic;
            _musicSilence = assetSilence; 
            _videoFilmGrain = assetVideo;
            _imageVignette = assetImage;
        }

        protected override void SetupTemplate()
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
            // Campaign XXX => MP4 Conversion 
            // **********************************
            #region ...
            for (var i = 0; i < VideoList.Count; i++)
            {
                if (VideoList[i] is Mp4)
                {
                    continue;
                }

                var videoConversion = Factory.CreateOutput<Mp4>();
               
                videoConversion.Add(VideoList[i]); 

                //FILTERS/SETTINGS
                var videoConversionSettings = SettingsCollection.ForOutput(
                    new OverwriteOutput(),
                    new BitRate(3000),
                    new FrameRate(29.97),
                    new VCodec(VideoCodecType.Libx264)
                );

                //FILTER APPLICATION
                videoConversion.Output.Settings = videoConversionSettings;

                Factory.AddToResources(videoConversion);

                VideoList[i] = videoConversion.Output.GetOutput();
            }
            #endregion


            // **********************************
            // Campaign mp3 => m4a (AAC Interview audio) 
            // **********************************
            #region ...
            var campaignMp3ToM4A = Factory.CreateOutput<M4A>();
            var campaignMp3ToM4AResource1 = campaignMp3ToM4A.Add(_musicSilence);
            var campaignMp3ToM4AReceipts = AudioList.Select(campaignMp3ToM4A.Add).ToList();
            CommandResourceReceipt lastMp3ToM4AReceipt = null;

            //FILTERS/SETTINGS
            var filterchainMp3ToM4A1 = Filterchain.FilterTo<M4A>(
                new Concat(1, 0)
            );
            var outputSettingsMp3ToM4A = SettingsCollection.ForOutput(
                new TrimShortest(),
                new OverwriteOutput(),
                new AudioBitRate(125),
                new ACodec(AudioCodecType.ExperimentalAac)
            );

            //FILTER APPLICATION
            campaignMp3ToM4AReceipts.ForEach(receipt =>
            {
                lastMp3ToM4AReceipt = lastMp3ToM4AReceipt == null ?
                    campaignMp3ToM4A.ApplyFilter(filterchainMp3ToM4A1, campaignMp3ToM4AResource1, receipt) :
                    campaignMp3ToM4A.ApplyFilter(filterchainMp3ToM4A1, lastMp3ToM4AReceipt, campaignMp3ToM4AResource1, receipt);
            });

            campaignMp3ToM4A.Output.Settings = outputSettingsMp3ToM4A;

            Factory.AddToResources(campaignMp3ToM4A);
            #endregion

            // **********************************
            // Campaign mp4 (480p resolution w/o audio)
            // **********************************
            #region ...
            var campaign480Mp4 = Factory.CreateOutput<Mp4>();
            var campaign480Mp4Receipts = VideoList.Select(campaign480Mp4.Add).ToList();
            CommandResourceReceipt last480Mp4Receipt = null;

            //FILTERS/SETTINGS
            var resolutionTemplate480 = new Obsolete.Resolution480P<Mp4>();
            var filterchain480Mp42 = Filterchain.FilterTo<Mp4>(
                new Crossfade(TimeSpan.FromSeconds(1), resolutionTemplate480)
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
                new Fade(FadeTransitionType.Out, 2)
            );
            var outputSettings480Mp4 = SettingsCollection.ForOutput(
                new RemoveAudio(),
                new TrimShortest(),
                new OverwriteOutput(),
                new BitRate(3000),
                new FrameRate(29.97),
                new AspectRatio(new FfmpegRatio(16, 9)), 
                new PixelFormat(PixelFormatType.Yuv420P),
                new VCodec(VideoCodecType.Libx264)
            );
            outputSettings480Mp4.MergeRange(resolutionTemplate480, FfmpegMergeOptionType.OldWins);

            //FILTER APPLICATION 
            var campaign480Mp4Concat = new List<CommandResourceReceipt>();
            //apply the cross fade filter and setup the concat chain.
            campaign480Mp4Receipts.ForEach(receipt =>
                {
                    if (last480Mp4Receipt == null)
                    {
                        campaign480Mp4Concat.Add(receipt);
                        last480Mp4Receipt = receipt;
                        return;
                    }

                    campaign480Mp4Concat.Add(campaign480Mp4.ApplyFilter(filterchain480Mp42, last480Mp4Receipt, receipt));
                    campaign480Mp4Concat.Add(receipt);
                    last480Mp4Receipt = receipt;
                });

            campaign480Mp4Concat = campaign480Mp4.ApplyFilterToEach<Mp4>(resolutionTemplate480, campaign480Mp4Concat);

            last480Mp4Receipt = campaign480Mp4.ApplyFilter(filterchain480Mp43, campaign480Mp4Concat);

            var assetImageReceipt = campaign480Mp4.Add(_imageVignette);
            var assetVideoReceipt = campaign480Mp4.Add(_videoFilmGrain);
            assetImageReceipt = campaign480Mp4.ApplyFilter<Mp4>(resolutionTemplate480, assetImageReceipt);
            assetVideoReceipt = campaign480Mp4.ApplyFilter<Mp4>(resolutionTemplate480, assetVideoReceipt);

            last480Mp4Receipt = campaign480Mp4.ApplyFilter(filterchain480Mp44, last480Mp4Receipt, assetImageReceipt);

            last480Mp4Receipt = campaign480Mp4.ApplyFilter(filterchain480Mp45, last480Mp4Receipt, assetVideoReceipt);

            campaign480Mp4.ApplyFilter(filterchain480Mp46, last480Mp4Receipt);

            campaign480Mp4.Output.Settings = outputSettings480Mp4;

            Factory.AddToResources(campaign480Mp4);
            #endregion

            // **********************************
            // Campaign mp4 (240p resolution w/o audio)
            // **********************************
            #region ...
            var campaign240Mp4 = Factory.CreateOutput<Mp4>();
            campaign240Mp4.Add(campaign480Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var resolutionTemplate240 = new Obsolete.Resolution240P<Mp4>();
            var outputSettings240Mp4 = SettingsCollection.ForOutput(
                new RemoveAudio(),
                new TrimShortest(),
                new OverwriteOutput(),
                new BitRate(1100),
                new FrameRate(29.97),
                new PixelFormat(PixelFormatType.Yuv420P),
                new VCodec(VideoCodecType.Libx264)
            );
            outputSettings240Mp4.MergeRange(resolutionTemplate240, FfmpegMergeOptionType.OldWins);

            //FILTER APPLICATION 
            campaign240Mp4.ApplyFilter<Mp4>(resolutionTemplate240);

            campaign240Mp4.Output.Settings = outputSettings240Mp4;

            Factory.AddToResources(campaign240Mp4);
            #endregion

            // **********************************
            // Campaign m4a (AAC Interview audio) 
            // **********************************
            #region ...
            var campaignM4AStartFade = (int)Helpers.GetLength(campaign480Mp4);
            var campaignM4A = Factory.CreateOutput<M4A>("audio_aac_" + Factory.Id + ".m4a");
            var campaignM4AResource1 = campaignM4A.Add(_musicBackground);
            var campaignM4AResource2 = campaignM4A.Add(campaignMp3ToM4A.Output.Resource);

            //FILTERS/SETTINGS
            var filterchainM4A1 = Filterchain.FilterTo<M4A>(
                new Volume(.3m)
            );
            var filterchainM4A2 = Filterchain.FilterTo<M4A>(
                new Volume(1.4m)
            );
            var filterchainM4A3 = Filterchain.FilterTo<M4A>(
                new AMix(DurationType.First)
            );
            var filterchainM4A4 = Filterchain.FilterTo<M4A>(
                new AFade(FadeTransitionType.Out, 2, campaignM4AStartFade - 2)
            );

            var outputSettingsM4A = SettingsCollection.ForOutput(
                new TrimShortest(),
                new OverwriteOutput(),
                new AudioBitRate(125),
                new ACodec(AudioCodecType.ExperimentalAac)
            );

            //FILTER APPLICATION
            var lastM4AReceipt1 = campaignM4A.ApplyFilter(filterchainM4A1, campaignM4AResource1);

            var lastM4AReceipt2 = campaignM4A.ApplyFilter(filterchainM4A2, campaignM4AResource2);

            lastM4AReceipt1 = campaignM4A.ApplyFilter(filterchainM4A3, lastM4AReceipt1, lastM4AReceipt2);

            campaignM4A.ApplyFilter(filterchainM4A4, lastM4AReceipt1);

            campaignM4A.Output.Settings = outputSettingsM4A;

            Factory.AddToOutput(campaignM4A);

            AudioOutput = campaignM4A.Output.Resource;
            #endregion

            // **********************************
            // Campaign mp4 (480p resolution w/audio)
            // **********************************
            #region ...
            var campaign480Mp4WAudio = Factory.CreateOutput<Mp4>("video_h264_480p_" + Factory.Id + ".mp4");
            campaign480Mp4WAudio.Add(campaignM4A.Output.Resource);
            campaign480Mp4WAudio.Add(campaign480Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var outputSettings480Mp4WAudio = SettingsCollection.ForOutput(
                new TrimShortest(),
                new OverwriteOutput(),
                new ACodec(AudioCodecType.Copy),
                new VCodec(VideoCodecType.Copy)
            );

            //FILTER APPLICATION 
            campaign480Mp4WAudio.Output.Settings = outputSettings480Mp4WAudio;

            Factory.AddToOutput(campaign480Mp4WAudio);

            Video480POutput = campaign480Mp4WAudio.Output.Resource;
            #endregion

            // **********************************
            // Campaign mp4 (240p resolution w/audio)
            // **********************************
            #region ...
            var campaign240Mp4WAudio = Factory.CreateOutput<Mp4>("video_h264_240p_" + Factory.Id + ".mp4");
            campaign240Mp4WAudio.Add(campaignM4A.Output.Resource);
            campaign240Mp4WAudio.Add(campaign240Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var outputSettings240Mp4WAudio = SettingsCollection.ForOutput(
                new TrimShortest(),
                new OverwriteOutput(),
                new ACodec(AudioCodecType.Copy),
                new VCodec(VideoCodecType.Copy)
            );

            //FILTER APPLICATION 
            campaign240Mp4WAudio.Output.Settings = outputSettings240Mp4WAudio;

            Factory.AddToOutput(campaign240Mp4WAudio);

            Video240POutput = campaign240Mp4WAudio.Output.Resource;
            #endregion

            // **********************************
            // Campaign Jpg (480p resolution w/audio)
            // **********************************
            #region ...
            var campaignJpg = Factory.CreateOutput<Jpg>("image_480p_" + Factory.Id + ".jpg");
            campaignJpg.Add(SettingsCollection.ForInput(
                new Duration(1),
                new SeekTo(TimeSpan.FromSeconds(5))
            ), campaign480Mp4.Output.Resource);

            //FILTERS/SETTINGS
            var outputSettingsJpg = SettingsCollection.ForOutput(
                new OverwriteOutput(),
                new FrameRate(1)
            );

            //FILTER APPLICATION 
            campaignJpg.Output.Settings = outputSettingsJpg;

            Factory.AddToOutput(campaignJpg);

            Image480POutput = campaignJpg.Output.Resource;
            #endregion
        }

        public IResource AudioOutput { get; set; }

        public IResource Video240POutput { get; set; }

        public IResource Video480POutput { get; set; }

        public IResource Image480POutput { get; set; }
    }
}
