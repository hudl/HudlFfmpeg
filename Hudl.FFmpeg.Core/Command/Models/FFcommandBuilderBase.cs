using System.Text;

namespace Hudl.FFmpeg.Command.Models
{
    public abstract class FFCommandBuilderBase 
    {
        protected readonly StringBuilder BuilderBase;

        protected FFCommandBuilderBase()
        {
            BuilderBase = new StringBuilder(100);            
        }

        public override string ToString()
        {
            return BuilderBase.ToString();
        }
    }
}
