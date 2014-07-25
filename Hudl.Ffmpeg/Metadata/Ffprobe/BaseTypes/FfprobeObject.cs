namespace Hudl.FFmpeg.Metadata.FFprobe.BaseTypes
{
    internal class FFprobeObject : IFFprobeValue
    {
        private FFprobeObject(object value)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            var ffprobeObject = obj as FFprobeObject;
            if (ffprobeObject == null)
            {
                return false;
            }

            return ffprobeObject.Value.Equals(Value); 
        }

        public static FFprobeObject Create(object value)
        {
            return new FFprobeObject(value);
        }
    }
}
