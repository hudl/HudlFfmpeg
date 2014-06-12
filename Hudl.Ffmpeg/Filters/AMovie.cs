using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// AMovie Audio filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class AMovie : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "amovie";

        public AMovie()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public AMovie(IAudio file)
            : this()
        {
            File = file;
        }

        public IAudio File { get; set; }

        public override void Validate()
        {
            if (File == null)
            {
                throw new InvalidOperationException("AMovie input cannot be nothing");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, "=", File.Path);
        }
    }
}
