using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    [AppliesToResource(Type = typeof(IVideo))]
    public class Scale : IFilter
    {
        private Dictionary<string, Point> _scalingPresets = new Dictionary<string,Point>() 
        {
            { "svga", new Point(800, 600) }, 
            { "xga", new Point(1024, 768) }, 
            { "ega", new Point(640, 350) }, 
            { "hd480", new Point(852, 480) }, 
            { "hd720", new Point(1280, 720) },
            { "hd1080", new Point(1920, 1080) }
        };

        public Scale()
        {
        }
        public Scale(string preset) : this() 
        {
            if (!_scalingPresets.ContainsKey(preset)) 
                throw new ArgumentException("The preset does not currently exist.", "preset"); 
            Width = _scalingPresets[preset].X;
            Height = _scalingPresets[preset].Y;
        }
        public Scale(int width, int height) : this() 
        {   
            if (width <= 0) 
                throw new ArgumentException("Width must be greater than zero for scaling.");
            if (height <= 0) 
                throw new ArgumentException("Height must be greater than zero for scaling.");
            Width = width;
            Height = height; 
        }

        public int Width { get; set; }

        public int Height { get; set; } 

        public string Type { get { return "scale"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString()
        {
            if (Width <= 0)
                throw new ArgumentException("Width must be greater than zero for scaling.");
            if (Height <= 0)
                throw new ArgumentException("Height must be greater than zero for scaling.");

            return string.Concat(Type, "=w=", Width, ":h=", Height);
        }
    }
}
