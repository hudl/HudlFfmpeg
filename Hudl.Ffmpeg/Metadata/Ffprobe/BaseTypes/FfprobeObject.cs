namespace Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes
{
    internal class FfprobeObject : IFfprobeValue
    {
        private FfprobeObject(object value)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            var ffprobeObject = obj as FfprobeObject;
            if (ffprobeObject == null)
            {
                return false;
            }

            return ffprobeObject.Value.Equals(Value); 
        }

        public static FfprobeObject Create(object value)
        {
            return new FfprobeObject(value);
        }
    }
}
