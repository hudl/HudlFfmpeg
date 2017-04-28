using Hudl.FFmpeg.Filters.Contexts;

namespace Hudl.FFmpeg.Filters.Interfaces
{
    public interface IFilterValidator
    {
        /// <summary>
        /// Validates the Filter based on the command and filterchain logic
        /// </summary>
        /// <param name="context">contains the current context for the filterchain and filter being validated</param>
        bool Validate(FilterValidatorContext context);
    }
}
