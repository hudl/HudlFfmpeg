namespace Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes
{
    internal class FfprobeKeyValuePair
    {
        private FfprobeKeyValuePair(string key, IFfprobeValue value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public IFfprobeValue Value { get; set; }

        public static FfprobeKeyValuePair Create(string key, object value)
        {
            var rawObject = FfprobeObject.Create(value);

            return new FfprobeKeyValuePair(key, rawObject);
        }

        public static FfprobeKeyValuePair Create(string key, IFfprobeValue value)
        {
            return new FfprobeKeyValuePair(key, value);
        }
    }
}
