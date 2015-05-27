using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.Serialization
{
    public class SettingSerializer
    {
        public static string Serialize(ISetting setting)
        {
            var settingData = GetSettingData(setting);
            var settingSerializer = new SettingSerializerWriter(settingData);

            return settingSerializer.Write();
        }

        private static SettingSerializerData GetSettingData(ISetting setting)
        {
            return SettingSerializerAttributeParser.GetSettingSerializerData(setting);
        }
    }
}
