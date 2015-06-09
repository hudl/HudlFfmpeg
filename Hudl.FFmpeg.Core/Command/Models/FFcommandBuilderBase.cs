using System.Text;

namespace Hudl.FFmpeg.Command.Models
{
    public abstract class FFcommandBuilderBase 
    {
        protected readonly StringBuilder BuilderBase;

        protected FFcommandBuilderBase()
        {
            BuilderBase = new StringBuilder(100);            
        }

        public override string ToString()
        {
            return BuilderBase.ToString();
        }
    }
}
