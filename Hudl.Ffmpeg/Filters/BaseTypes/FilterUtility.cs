using System.Text;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    internal class FilterUtility
    {
        /// <summary>
        /// used to concatenate the type and parameters in a single standard implementation
        /// </summary>
        public static string JoinTypeAndParameters(IFilter filter, StringBuilder builder)
        {
            if (builder.Length == 0)
            {
                return filter.Type;
            }

            return string.Format("{0}={1}",
                    filter.Type,
                    builder);
        }

        /// <summary>
        /// used to attach parameters to a builder string for filters, so that they may meet syntax requirements
        /// </summary>
        public static void ConcatenateParameter(StringBuilder builder, string paramName, object paramValue)
        {
            builder.AppendFormat("{2}{0}={1}",
                    paramName,
                    paramValue,
                    (builder.Length > 0) ? ":" : string.Empty);
        }

        /// <summary>
        /// used to attach parameters to a builder string for filters, so that they may meet syntax requirements
        /// </summary>
        public static void ConcatenateParameter(StringBuilder builder, object paramValue)
        {
            builder.AppendFormat("{1}{0}",
                    paramValue,
                    (builder.Length > 0) ? ":" : string.Empty);
        }
    }
}
