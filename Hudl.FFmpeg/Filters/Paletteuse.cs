using System.Drawing;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Filter that uses the  Paletteuse file 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "paletteuse", MinInputs = 2, MaxInputs = 2)]
    public class Paletteuse : IFilter
    {
    }
}
