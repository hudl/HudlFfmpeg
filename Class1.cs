using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Templates;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Resources;

namespace Hudl.Ffmpeg
{
    public class Class1
    {
        public static void CreateProject()
        {
            CampaignProject project = new CampaignProject();
            project.Add(new Mp4());
            project.Add(new Mp4());
            project.Add(new Mp3());

            var Video = project.Render<BatchCommandProcessorReciever>(); 

        } 

    }
}
