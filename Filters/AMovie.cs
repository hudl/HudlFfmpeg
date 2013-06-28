using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// AMovie Audio filter declares a filter resource that can be given a specific map.
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class AMovie : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "amovie";

        public AMovie(IAudio file)
            : base(FilterType, FilterMaxInputs)
        {
            File = file;
        }

        public IAudio File { get; set; }

        public override string ToString()
        {
            if (File == null)
            {
                throw new ArgumentException("AMovie input cannot be nothing");
            }

            return string.Concat(Type, "=", File.Path);
        }
    }
}
