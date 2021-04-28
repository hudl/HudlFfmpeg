using System.Drawing;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Filter that generates Palette file 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "palettegen", MinInputs = 1, MaxInputs = 1)]
    public class Palettegen : IFilter
    {
    }
}
