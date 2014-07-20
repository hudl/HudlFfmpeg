
using System;
using System.Text;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public abstract class BaseMovie : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "movie";

        protected BaseMovie(string filterPrefix)
            : base(string.Concat(filterPrefix, FilterType), FilterMaxInputs)
        {
        }

        public IContainer Resource { get; set; }

        public string FormatName { get; set; }

        public double? SeekPoint { get; set; }

        public string Streams { get; set; }

        public int? StreamIndex { get; set; }

        public int? Loop { get; set; }

        public override void Validate()
        {
            if (Resource == null)
            {
                throw new InvalidOperationException("A resource is required for movie filters.");
            }

            if (SeekPoint.HasValue && SeekPoint < 0)
            {
                throw new InvalidOperationException("Seek point greater than or equal to zero is required for movie filters.");
            }

            if (StreamIndex.HasValue && StreamIndex < 0)
            {
                throw new InvalidOperationException("Stream index greater than or equal to zero is required for movie filters.");
            }

            if (Loop.HasValue && Loop < 1)
            {
                throw new InvalidOperationException("Loop greater than zero is required for movie filters.");
            }
        }

        public override string ToString()
        {
            var filterParameters = new StringBuilder(100);

            if (Resource != null)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "filename", Resource.Path); 
            }

            if (!string.IsNullOrWhiteSpace(FormatName))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "f", FormatName); 
            }

            if (SeekPoint.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "sp", SeekPoint.GetValueOrDefault());
            }

            if (!string.IsNullOrWhiteSpace(Streams))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "s", Streams); 
            }

            if (StreamIndex.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "si", StreamIndex.GetValueOrDefault());
            }

            if (Loop.HasValue)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "loop", Loop.GetValueOrDefault());
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
