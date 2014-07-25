using System;
using System.Globalization;
using Hudl.FFmpeg.Resources.BaseTypes;
using Microsoft.SqlServer.Server;

namespace Hudl.FFmpeg.Common
{
    internal class Formats
    {
        private const string ExperimentalIndentifier = "experimental";
        private const string ExperimentalQualifier = " -strict experimental";

        private const string EnumSlashIdentifier = "_";
        private const string EnumSlashQualifier = "/";

        public static string Duration(int seconds)
        {
            return Duration(new TimeSpan(0, 0, seconds));
        }
        public static string Duration(TimeSpan timespan)
        {
            return string.Format("{0}:{1}:{2}.{3}",
                                 timespan.Hours.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                                 timespan.Minutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                                 timespan.Seconds.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), 
                                 timespan.Milliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public static string Map(IStream stream, int index)
        {
            return Map(string.Concat(index, ":", stream.ResourceIndicator));
        }
        public static string Map(IStream stream)
        {
            return Map(stream.Map);
        }
        public static string Map(string map, bool forSettings = false)
        {
            return forSettings && map.IndexOf(":", StringComparison.InvariantCulture) > -1
                       ? map
                       : string.Format("[{0}]", map);
        }

        public static string Library(string codec)
        {
            var codecString = codec.ToLower();
            if (codecString.IndexOf(ExperimentalIndentifier, StringComparison.Ordinal) != -1)
            {
                codecString = codecString.Replace(ExperimentalIndentifier, string.Empty);
                codecString = string.Concat(codecString, ExperimentalQualifier);
            }

            return codecString;
        }
        public static string Library(FormatType codec)
        {
            return Library(codec.ToString());
        }
        public static string Library(AudioCodecType codec)
        {
            return Library(codec.ToString());
        }
        public static string Library(VideoCodecType codec)
        {
            return Library(codec.ToString());
        }
        public static string Library(PixelFormatType library)
        {
            return Library(library.ToString());
        }

        public static string EnumValue<TValue>(TValue enumValue, bool convertIdentifiers = false)
        {
            var enumString = enumValue.ToString().ToLower();
            if (convertIdentifiers && enumString.IndexOf(EnumSlashIdentifier, StringComparison.Ordinal) != -1)
            {
                enumString = enumString.Replace(EnumSlashIdentifier, EnumSlashQualifier);
            }

            return enumString;
        }

        public static string EscapeString(string value)
        {
            return string.Format("'{0}'", value);
        }
    }
}
