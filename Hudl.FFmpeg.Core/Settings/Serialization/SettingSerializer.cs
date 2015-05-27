using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings.Serialization
{
    public class SettingSerializer
    {
        public static string Serialize(ISetting setting)
        {
            var filterData = GetSettingData(setting);
            var filterSerializer = new SettingSerializerWriter(filterData);

            return filterSerializer.Write();
        }

        private static SettingSerializerData GetSettingData(ISetting setting)
        {
            return SettingSerializerAttributeParser.GetSettingSerializerData(setting);
        }
    }
}
