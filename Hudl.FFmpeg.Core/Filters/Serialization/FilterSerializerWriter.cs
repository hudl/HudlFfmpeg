using System.Text;

namespace Hudl.FFmpeg.Filters.Serialization
{
    internal class FilterSerializerWriter
    {
        private readonly FilterSerializerData _filterData;

        public FilterSerializerWriter(FilterSerializerData filterData)
        {
            _filterData = filterData; 
        }

        public string Write()
        {
            var parameterBuilder = new StringBuilder(75); 

            _filterData.Parameters.ForEach(fpd =>
            {
                if (fpd.IsDefault || string.IsNullOrWhiteSpace(fpd.Value))
                {
                    return;
                }

                ConcatenateParameter(parameterBuilder, fpd.Name, fpd.Value);
            });

            return JoinTypeAndParameters(parameterBuilder);
        }

        /// <summary>
        /// used to concatenate the type and parameters in a single standard implementation
        /// </summary>
        private string JoinTypeAndParameters(StringBuilder builder)
        {
            if (builder.Length == 0)
            {
                return _filterData.Filter.Name;
            }

            return string.Format("{0}={1}",
                    _filterData.Filter.Name,
                    builder);
        }

        /// <summary>
        /// used to attach parameters to a builder string for filters, so that they may meet syntax requirements
        /// </summary>
        private void ConcatenateParameter(StringBuilder builder, string paramName, object paramValue)
        {
            builder.AppendFormat("{2}{0}={1}",
                    paramName,
                    paramValue,
                    (builder.Length > 0) ? ":" : string.Empty);
        }

        /// <summary>
        /// used to attach parameters to a builder string for filters, so that they may meet syntax requirements
        /// </summary>
        private void ConcatenateParameter(StringBuilder builder, object paramValue)
        {
            builder.AppendFormat("{1}{0}",
                    paramValue,
                    (builder.Length > 0) ? ":" : string.Empty);
        }
    }
}
