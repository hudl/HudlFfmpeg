using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Movie Video filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    [AppliesToResource(Type=typeof(IImage))]
    public class Movie : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "movie";

        public Movie()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Movie(IResource file)
            : this()
        {
            File = file;
        }

        public IResource File { get; set; }

        public override string ToString()
        {
            if (File == null)
            {
                throw new InvalidOperationException("Movie input cannot be nothing");
            }

            return string.Concat(Type, "=", File.Path);
        }
    }
}
