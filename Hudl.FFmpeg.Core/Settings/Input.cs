using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Formatters;
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

        [SettingValue(Formatter = typeof(LocalUriFormatter))]
        public IContainer Resource { get; protected set; }
    }
}
