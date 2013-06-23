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
    [AppliesToResource(Type=typeof(IAudio))]
    public class AMovie : IFilter
    {
        public AMovie(IAudio file)
        {
            File = file;
        }

        public IAudio File { get; set; }

        public string Type { get { return "amovie"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString()
        {
            if (File == null) 
                throw new ArgumentException("AMovie input cannot be nothing", "File");

            return string.Concat(Type, "=", File.Path);
        }
    }
}
