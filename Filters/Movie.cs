using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    [AppliesToResource(Type=typeof(IVideo))]
    [AppliesToResource(Type=typeof(IImage))]
    public class Movie : IFilter
    {
        public Movie(IResource file)
        {
            File = file;
        }

        public IResource File { get; set; }

        public string Type { get { return "movie"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString()
        {
            if (File == null) 
                throw new ArgumentException("Movie input cannot be nothing", "File");

            return string.Concat(Type, "=", File.Path);
        }
    }
}
