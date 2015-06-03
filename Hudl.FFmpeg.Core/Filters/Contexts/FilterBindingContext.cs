namespace Hudl.FFmpeg.Filters.Contexts
{
    public class FilterBindingContext
    {
        private FilterBindingContext(int? numberOfStreamsIn, int? numberOfStreamsOut)
        {
            NumberOfStreamsIn = numberOfStreamsIn;
            NumberOfStreamsOut = numberOfStreamsOut;
        }

        public int? NumberOfStreamsIn { get; set; }

        public int? NumberOfStreamsOut { get; set; }

        public static FilterBindingContext Empty()
        {
            return new FilterBindingContext(null, null);
        }

        public static FilterBindingContext Create(int numberOfStreamsIn, int numberOfStreamsOut)
        {
            return new FilterBindingContext(numberOfStreamsIn, numberOfStreamsOut);
        }
    }
}
