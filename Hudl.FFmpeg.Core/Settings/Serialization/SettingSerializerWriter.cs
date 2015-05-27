using System.Text;

namespace Hudl.FFmpeg.Settings.Serialization
{
    internal class SettingSerializerWriter
    {
        private readonly SettingSerializerData _settingData;

        public SettingSerializerWriter(SettingSerializerData settingData)
        {
            _settingData = settingData; 
        }

        public string Write()
        {
            return string.IsNullOrWhiteSpace(_settingData.Value.Value) 
                ? ConcatenateSetting(_settingData.Setting.Name) 
                : ConcatenateSetting(_settingData.Setting.Name, _settingData.Value.Value);
        }

        private static string ConcatenateSetting(string paramName, object paramValue)
        {
            return string.Format("-{0} {1}", paramName, paramValue);
        }

        private static string ConcatenateSetting(string paramName)
        {
            return string.Format("-{0}", paramName);
        }
    }
}
