using System.Collections.Generic;
using Hudl.FFmpeg.Settings.Attributes;

namespace Hudl.FFmpeg.Settings.Serialization
{
    internal class SettingSerializerData
    {
        public SettingSerializerData()
        {
            Parameters = new List<SettingSerializerDataParameter>();
        }

        public SettingAttribute Setting { get; set; }

        public List<SettingSerializerDataParameter> Parameters { get; set; }
    }
}
