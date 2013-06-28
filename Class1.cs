using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Templates;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Filters.Templates;
using Hudl.Ffmpeg.Resources;

namespace Hudl.Ffmpeg
{
    public class Class1
    {
        private const string BlendExpression = "A*(6/10)+B*(1-(6/10))";
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

        public void CreateProject()
        {
            //*******************************************
            //   TEST #1 : Ffmpeg framework using campaign template.
            //
            // READ THIS FIRST:
            //  - I am looking for what you believe it is doing and if it makes sense to you. 
            //  - The variable names are non-descriptive for the usability test to ensure the calls are self explanitory. 
            //  - The commands below should be sufficient enough to describe the process without knowledge of each object
            //*******************************************
            var campaignProject = new CampaignProject();
            campaignProject.Add(new Mp4("c:/source/movie1.mp4"));
            campaignProject.Add(new Mp4("c:/source/movie2.mp4"));
            campaignProject.Add(new Mp4("c:/source/movie3.mp4"));
            campaignProject.Add(new Mp3("c:/source/audio1.mp3"));
            campaignProject.Render<BatchCommandProcessorReciever>();

            
            //*******************************************
            //   TEST #3 : Ffmpeg framework using project base.
            //
            // READ THIS FIRST:
            //  - I am looking for what you believe it is doing and if it makes sense to you. 
            //  - The variable names are non-descriptive for the usability test to ensure the calls are self explanitory. 
            //  - The commands below should be sufficient enough to describe the process without knowledge of each object
            //*******************************************
            var factory = new CommandFactory();
            
           
            
            
            
            
            var campaignVideo = factory.OutputAs<Mp4>();
            var campaignAudio = factory.OutputAs<M4A>();
            var campaignMobile = factory.OutputAs<Mp4>();
            var campaignImage = factory.OutputAs<Png>();

            //*******************************************
            // VIDEO PREPWORK
            //*******************************************
            var videoResources = new List<IResource>
                {
                    new Mp4("c:\source\campaigns\test\vid001.mp4", TimeSpan.FromSeconds(15)),
                    new Mp4("c:\source\campaigns\test\vid002.mp4", TimeSpan.FromSeconds(9)),
                    new Mp4("c:\source\campaigns\test\vid003.mp4", TimeSpan.FromSeconds(9)),
                    new Mp4("c:\source\campaigns\test\vid004.mp4", TimeSpan.FromSeconds(11)),
                    new Mp4("c:\source\campaigns\test\vid005.mp4", TimeSpan.FromSeconds(12))
                };
            for (var i = 0; i < videoResources.Count; i++)
            {
                
            }
            var prepatoryCommands = new List<Command<IResource>>();

            
            //*******************************************
            // VIDEO FILTERS\SETTINGS
            //*******************************************
            #region ...
            var filterVideoStep1 = new Filterchain<Mp4>(
                new Scale(ScalePresetTypes.Hd720),
                new SetDar(new FfmpegRatio(16, 9)),
                new SetSar(new FfmpegRatio(1, 1))
            );
            var filterVideoStep2 = new Filterchain<Mp4>(
                new Crossfade(TimeSpan.FromSeconds(1))
            );
            var filterVideoStep3 = new Filterchain<Mp4>(
                new Concat()
            );
            var filterVideoStep4 = new Filterchain<Mp4>(
                new Overlay()
            );
            var filterVideoStep5 = new Filterchain<Mp4>(
                new Blend(BlendExpression),
                new ColorBalance(_shadows, _midtones, _highlights),
                new Fade(Fade.FadeType.Out, 15, 2)
            );
            var videoSettings = new SettingsCollection(
                new OverwriteOutput(), 
                new VCodec(VideoCodecTypes.Libx264), 
                new FrameRate(29.97), 
                new BitRate(3000), 
                new Dimensions(1280, 720), 
                new RemoveAudio(), 
                new PixelFormat(PixelFormatTypes.Yuv420P), 
                new TrimShortest()
            );
            #endregion

            //*******************************************
            // AUDIO FILTERS\SETTINGS
            //*******************************************
            #region ...
            var filterAudioStep1 = new Filterchain<M4A>(
                new Volume(.2m)
            );
            var filterAudioStep2 = new Filterchain<M4A>(
                new Concat(), 
                new Volume(1.5m)
            );
            var filterAudioStep3 = new Filterchain<M4A>(
                new AMix() { Duration = AMix.DurationType.First }, 
                new AFade(AFade.FadeType.Out, 15, 2)
            );
            var audioSettings = new SettingsCollection(
                new OverwriteOutput(), 
                new TrimShortest(), 
                new ACodec(AudioCodecTypes.ExperimentalAac)
            );
            #endregion

            //*******************************************
            // CAMPAIGN VIDEO GENERATION
            //*******************************************
            #region ...
            var campaignReceipts = new List<CommandResourceReceipt>()
                {
                    campaignVideo.Add<Mp4>("c:/source/movie1.mp4"),
                    campaignVideo.Add<Mp4>("c:/source/movie2.mp4"),
                    campaignVideo.Add<Mp4>("c:/source/movie3.mp4"), 
                    campaignVideo.Add<Png>("c:/source/assets/vignette.png"),
                    campaignVideo.Add<Mp4>("c:/source/assets/filmgrain.mp4")
                };

            campaignVideo.ApplyFilterToEach(filterVideoStep1); 

            campaignVideo.ApplyFilter(filterVideoStep2, campaignReceipts[0], campaignReceipts[1]);
            campaignVideo.ApplyFilter(filterVideoStep2, campaignReceipts[1], campaignReceipts[2]);

            var concatReceipt = campaignVideo.ApplyFilter(filterVideoStep3);

            var overlayReceipt = campaignVideo.ApplyFilter(filterVideoStep4, concatReceipt, campaignReceipts[3]);

            campaignVideo.ApplyFilter(filterVideoStep5, overlayReceipt, campaignReceipts[4]);

            campaignVideo.Output.Settings = videoSettings;
            #endregion

            //*******************************************
            // CAMPAIGN AUDIO GENERATION
            //*******************************************
            #region ...
            var audioReceipts = new List<CommandResourceReceipt>()
                {
                    campaignAudio.Add<Mp4>("c:/source/audio.mp3"),
                    campaignAudio.Add<Png>("c:/source/assets/silence.m4a"),
                    campaignAudio.Add<Png>("c:/source/assets/background.m4a")
                };

            var recieptAudio1 = campaignAudio.ApplyFilter(filterAudioStep1, audioReceipts[2]);

            var receiptAudio2 = campaignAudio.ApplyFilter(filterAudioStep2, audioReceipts[0], audioReceipts[1]);

            campaignAudio.ApplyFilter(filterAudioStep3, recieptAudio1, receiptAudio2);
            #endregion

            //*******************************************
            // CAMPAIGN MOBILE GENERATION
            //*******************************************
            #region ...
            var mobileReceipts = new List<CommandResourceReceipt>()
                {
                    campaignAudio.Add(campaignVideo.Output),
                    campaignMobile.Add(campaignAudio.Output)
                };
            #endregion

            //*******************************************
            // CAMPAIGN IMAGE GENERATION
            //*******************************************
            #region ...
            var imageReceipts = new List<CommandResourceReceipt>()
                {
                    campaignImage.Add(campaignVideo.Output),
                };
            #endregion

            ResourceList output = project.Render<BatchCommandProcessorReciever>(); 
        } 

    }
}
