using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes; 
using Hudl.Ffmpeg.Resources; 

namespace Hudl.Ffmpeg.Templates
{
    public class CampaignProject : 
        IProject<Mp4>
    {
        public IReadOnlyList<Resources.BaseTypes.IResource> Resources
        {
            get { throw new NotImplementedException(); }
        }

        public IProject Add<TypeA>(TypeA resource) where TypeA : Resources.BaseTypes.IResource
        {
            throw new NotImplementedException();
        }

        public IProject Insert<TypeA>(TypeA resource, int index) where TypeA : Resources.BaseTypes.IResource
        {
            throw new NotImplementedException();
        }

        public IProject RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetLength()
        {
            throw new NotImplementedException();
        }

        public void Render<TypeP>() where TypeP : Command.BaseTypes.ICommandProcessor, new()
        {
            throw new NotImplementedException();
        }

        public Filters.BaseTypes.Filtergraph Filtergraph
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
