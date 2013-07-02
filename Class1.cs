using System;
using System.Linq;
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
            campaignProject.Render();
        } 

    }
}
