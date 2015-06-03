using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// input file name
    /// </summary>
    [Setting(Name = "i", IsMultipleAllowed = true)]
    public class Input : ISetting
    {
        public Input(IContainer resource)
        {
            Resource = resource; 
        }
          
        [SettingParameter(Formatter = typeof(LocalUriFormatter))]
        [Validate(LogicalOperators.NotEquals, null)]
        public IContainer Resource { get; protected set; }
    }
}
