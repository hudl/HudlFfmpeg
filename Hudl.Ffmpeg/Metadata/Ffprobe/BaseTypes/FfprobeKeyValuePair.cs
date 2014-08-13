namespace Hudl.FFmpeg.Metadata.FFprobe.BaseTypes
{
    internal class FFprobeKeyValuePair
    {
        private FFprobeKeyValuePair(string key, IFFprobeValue value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public IFFprobeValue Value { get; set; }

        public static FFprobeKeyValuePair Create(string key, object value)
        {
            var rawObject = FFprobeObject.Create(value);

            return new FFprobeKeyValuePair(key, rawObject);
        }

        public static FFprobeKeyValuePair Create(string key, IFFprobeValue value)
        {
            return new FFprobeKeyValuePair(key, value);
        }
    }
}
