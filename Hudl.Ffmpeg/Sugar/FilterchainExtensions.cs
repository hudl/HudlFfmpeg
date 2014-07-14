using System.Linq;
using Hudl.Ffmpeg.Filters.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class FilterchainExtensions
    {
        public static Filterchain CloneAndEmpty(this Filterchain filterchain)
        {
            var newOutputs = filterchain.OutputList.Select(output => output.Copy().Resource).ToList();

            return Filterchain.Create(newOutputs); 
        }
    }
}
