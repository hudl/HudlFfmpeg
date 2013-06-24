using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Resources.BaseTypes; 

namespace Hudl.Ffmpeg.Resources
{
    public class ResourceList
        : IResource
    {
        private new List<IResource> _outputResourceList;

        public string Map
        {
            get
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
            set
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
        }

        public string Path
        {
            get
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
            set
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
        }

        public string Format
        {
            get 
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
        }

        public TimeSpan Length
        {
            get
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
            set
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
        }

        public TimeSpan StartAt
        {
            get
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
            set
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
        }

        public TimeSpan EndAt
        {
            get
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
            set
            {
                throw new NotImplementedException("Resource list contains multiple resources, refer to each individually.");
            }
        }
    }
}
