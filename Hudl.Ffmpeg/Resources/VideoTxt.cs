using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Resources
{
    public class Txt : BaseVideo
    {
        private const string FileFormat = ".txt";

        public Txt()
            : base(FileFormat)
        {
        }

        protected override IResource InstanceOfMe()
        {
            return new Txt
                {
                    Id = Id,
                    Length = Length,
                    Name = Name,
                    Path = Path
                };
        }
    }
}