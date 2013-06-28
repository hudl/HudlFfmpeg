
namespace Hudl.Ffmpeg.Command
{
    public class CommandResourceReceipt
    {
        internal CommandResourceReceipt(string map)
        {
            Map = map;
        }

        public string Map { get; protected set; }
    }
}
