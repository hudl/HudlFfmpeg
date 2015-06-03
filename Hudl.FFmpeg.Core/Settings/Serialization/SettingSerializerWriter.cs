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
            var parameterBuilder = new StringBuilder(75);

            if (!_settingData.Setting.IsParameterless)
            {
                _settingData.Parameters.ForEach(spd =>
                    {
                        if (string.IsNullOrWhiteSpace(spd.Value))
                        {
                            return;
                        }

                        parameterBuilder.Append(spd.Value);
                    });
            }

            return parameterBuilder.Length == 0 
                ? ConcatenateSetting(_settingData.Setting.Name)
                : ConcatenateSetting(_settingData.Setting.Name, parameterBuilder.ToString());
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
