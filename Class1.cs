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
            CampaignProject campaignProject = new Templates.CampaignProject();
            campaignProject.Add(new Mp4("c:/source/movie1.mp4"));
            campaignProject.Add(new Mp4("c:/source/movie2.mp4"));
            campaignProject.Add(new Mp4("c:/source/movie3.mp4"));
            campaignProject.Add(new Mp3("c:/source/audio1.mp3"));
            ResourceList outputResources = campaignProject.Render<BatchCommandProcessorReciever>();




            //*******************************************
            //   TEST #2 : Ffmpeg framework using project base.
            //
            // READ THIS FIRST:
            //  - I am looking for what you believe it is doing and if it makes sense to you. 
            //  - The variable names are non-descriptive for the usability test to ensure the calls are self explanitory. 
            //  - The commands below should be sufficient enough to describe the process without knowledge of each object
            //*******************************************
            Project project = new Project();
            Mp4 videoIn1 = project.Add<Mp4>("c:/source/movie1.mp4");
            Mp4 videoIn2 = project.Add<Mp4>("c:/source/movie2.mp4");
            Mp4 videoIn3 = project.Add<Mp4>("c:/source/movie3.mp4");
            Mp3 audioIn1 = project.Add<Mp3>("c:/source/audio1.mp3");
            M4a backgroundAudio = project.Add<M4a>("background.m4a"); 
            Png overlayImage = project.Add<Png>("vignette.png");
            Mp4 overlayVideo = project.Add<Mp4>("filmgrain.mp4");

            Filterchain<Mp4> filterchainFormat = new Filterchain<Mp4>(
                    new Scale("hd720"),
                    new SetDar(new FfmpegRatio(16, 9)),
                    new SetSar(new FfmpegRatio(1, 1))
                );
            Mp4 filterResult1 = project.Filtergraph.Assign(filterchainFormat, videoIn1);
            Mp4 filterResult2 = project.Filtergraph.Assign(filterchainFormat, videoIn2);
            Mp4 filterResult3 = project.Filtergraph.Assign(filterchainFormat, videoIn3);

            Filterchain<Mp4> filterchainCrossFade = new Filterchain<Mp4>(
                    new Crossfade()
                ); 
            Mp4 filterResult1to2 = project.Filtergraph.Assign(filterchainCrossFade, filterResult1, filterResult2);
            Mp4 filterResult2to3 = project.Filtergraph.Assign(filterchainCrossFade, filterResult2, filterResult3);

            Filterchain<Mp4> filterchainConcat = new Filterchain<Mp4>(
                    new Concat(5, 0, 1)
                ); 
            Mp4 filterResult4 = project.Filtergraph.Assign(filterchainConcat, filterResult1, 
                                                                              filterResult1to2, 
                                                                              filterResult2, 
                                                                              filterResult2to3, 
                                                                              filterResult3);

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

            project.MarkAsOutput(outputFile1);
            project.MarkAsOutput(outputFile2);
            project.MarkAsOutput(outputFile3);
            project.MarkAsOutput(outputFile4);


            ResourceList output = project.Render<BatchCommandProcessorReciever>(); 
        } 

    }
}
