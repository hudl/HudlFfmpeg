using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
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
            var campaignProject = new Templates.CampaignProject();
            campaignProject.Add(new Mp4("c:/source/movie1.mp4"));
            campaignProject.Add(new Mp4("c:/source/movie2.mp4"));
            campaignProject.Add(new Mp4("c:/source/movie3.mp4"));
            campaignProject.Add(new Mp3("c:/source/audio1.mp3"));
            var outputResources = campaignProject.Render<BatchCommandProcessorReciever>();



            //*******************************************
            //   TEST #2 : Ffmpeg framework using project base.
            //
            // READ THIS FIRST:
            //  - I am looking for what you believe it is doing and if it makes sense to you. 
            //  - The variable names are non-descriptive for the usability test to ensure the calls are self explanitory. 
            //  - The commands below should be sufficient enough to describe the process without knowledge of each object
            //*******************************************
            var project = new Project();
            var videoIn1 = project.Add<Mp4>("c:/source/movie1.mp4");
            var videoIn2 = project.Add<Mp4>("c:/source/movie2.mp4");
            var videoIn3 = project.Add<Mp4>("c:/source/movie3.mp4");
            var audioIn1 = project.Add<Mp3>("c:/source/audio1.mp3");
            var backgroundAudio = project.Add<M4a>("background.m4a"); 
            var overlayImage = project.Add<Png>("vignette.png");
            var overlayVideo = project.Add<Mp4>("filmgrain.mp4");

            var filterchainFormat = new Filterchain<Mp4>(
                    new Scale("hd720"),
                    new SetDar(new FfmpegRatio(16, 9)),
                    new SetSar(new FfmpegRatio(1, 1))
                );
            var filterResult1 = project.ApplyFilter(filterchainFormat, videoIn1);
            var filterResult2 = project.ApplyFilter(filterchainFormat, videoIn2);
            var filterResult3 = project.ApplyFilter(filterchainFormat, videoIn3);

            var filterchainCrossFade = new Filterchain<Mp4>(
                    new Crossfade()
                );
            project.ApplyFilter(new Crossfade(), filterResult1, filterResult2); 

            Mp4 filterResult1To2 = project.Filtergraph.Assign(filterchainCrossFade, filterResult1, filterResult2);
            Mp4 filterResult2To3 = project.Filtergraph.Assign(filterchainCrossFade, filterResult2, filterResult3);

            Filterchain<Mp4> filterchainConcat = new Filterchain<Mp4>(
                    new Concat(5, 0, 1)
                ); 
            Mp4 filterResult4 = project.Filtergraph.Assign(filterchainConcat, filterResult1, 
                                                                              filterResult1To2, 
                                                                              filterResult2, 
                                                                              filterResult2To3, 
                                                                              filterResult3);

            var f = Filterchain.ToOutput<Mp3>(); 

            Filterchain<Mp4> filterchainVignette = new Filterchain<Mp4>(
                    new Overlay()
                ); 
            Mp4 filterResult5 = project.Filtergraph.Assign(filterchainVignette, overlayImage, filterResult4);

            Filterchain<Mp4> filterchainGrain = new Filterchain<Mp4>(
                    new Blend(BlendExpression),
                    new ColorBalance(_shadows, _midtones, _highlights),
                    new Fade(Fade.FadeType.@out, 15, 2)
                ); 
            var outputFile1 = project.Filtergraph.Assign(filterchainGrain, filterResult5);

            Filterchain<M4a> filterchainAudio = new Filterchain<M4a>(
                    new 
                );

            project.SetOutput(outputFile1);
            project.SetOutput(outputFile2);
            project.SetOutput(outputFile3);
            project.SetOutput(outputFile4);

            ResourceList output = project.Render<BatchCommandProcessorReciever>(); 
        } 

    }
}
