namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public interface IFilterParameterValidator
    {
        bool Validate(object value);
    }
}
