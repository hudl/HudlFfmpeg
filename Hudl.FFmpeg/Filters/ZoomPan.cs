using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using System.Collections.Generic;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Applies Zoom & Pan effect.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name ="zoompan", MinInputs = 1, MaxInputs = 1)]
    public class ZoomPan : IFilter
    {
        public ZoomPan()
        {

        }

        public ZoomPan(string zoom, string x, string y, string d, ScalePresetType? size)
        {
            Zoom = zoom;
            X = x;
            Y = y;
            D = d;
            S = size.HasValue ? size.Value : ScalePresetType.Hd720;
        }

        [FilterParameter(Name = "zoom", Formatter = typeof(SingleQuoteFormatter))]
        public string Zoom { get; set; }

        [FilterParameter(Name = "x", Formatter = typeof(SingleQuoteFormatter))]
        public string X { get; set; }

        [FilterParameter(Name = "y", Formatter = typeof(SingleQuoteFormatter))]
        public string Y { get; set; }

        [FilterParameter(Name = "d")]
        public string D { get; set; }

        [FilterParameter(Name = "s", Default = ScalePresetType.Hd720)]
        public ScalePresetType S { get; set; }

        [FilterParameter(Name = "fps", Default = 25)]
        public double Fps { get; set; }

        public virtual MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            //To-do: Need to fill this in.
            return infoToUpdate;
        }
    }
}
