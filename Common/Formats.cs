using System;
using System.Globalization;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Common
{
    internal class Formats
    {
        private const string ExperimentalIndentifier = "experimental";
        private const string ExperimentalQualifier = " -strict experimental";

        public static string Duration(int seconds)
        {
            return Duration(new TimeSpan(0, 0, seconds));
        }
        public static string Duration(TimeSpan timespan)
        {
            return string.Format("{0}:{1}:{2}",
                                 timespan.Hours.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                                 timespan.Minutes.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                                 timespan.Seconds.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
        }
        public static string Map(IResource input, int index)
        {
            return Map(string.Concat(index, ":", input.ResourceIndicator));
        }
        public static string Map(IResource input)
        {
            return Map(input.Map);
        }
        public static string Map(string map)
        {
            return string.Format("[{0}]", map);
        }
        private static string Library(string codec)
        {
            var codecString = codec.ToLower();
            if (codecString.IndexOf(ExperimentalIndentifier, StringComparison.Ordinal) != -1)
            {
                codecString = codecString.Replace(ExperimentalIndentifier, string.Empty);
                codecString = string.Concat(codecString, ExperimentalQualifier);
            }

            return codecString;
        }
        public static string Library(AudioCodecTypes codec)
        {
            return Library(codec.ToString());
        }
        public static string Library(VideoCodecTypes codec)
        {
            return Library(codec.ToString());
        }
        public static string Library(PixelFormatTypes library)
        {
            return Library(library.ToString());
        }
    }
}
