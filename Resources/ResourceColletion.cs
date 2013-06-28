using System.Collections.Generic;
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.Resources
{
    /// <summary>
    /// this is a list of resource files used to describe a returned collection of files
    /// </summary>
    public class ResourceColletion
    {
        public ResourceColletion()
        {
            Resources = new List<IResource>();
        }

        public List<IResource> Resources { get; protected set; }
    }
}
