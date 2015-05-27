using Hudl.FFmpeg.Settings.Attributes;

namespace Hudl.FFmpeg.Settings.Serialization
{
    internal class SettingSerializerData
    {
        public SettingSerializerData()
        {
        }

        public SettingAttribute Setting { get; set; }

        public SettingSerializerDataParameter Value { get; set; }
    }
}
