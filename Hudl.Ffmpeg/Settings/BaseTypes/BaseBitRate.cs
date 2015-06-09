using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseBitRate : ISetting
    {
        protected BaseBitRate(int rate)
        {
            Rate = rate;
        }

        [SettingParameter(Formatter = typeof(Int32ToKbs))]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int Rate { get; set; }
    }
}
