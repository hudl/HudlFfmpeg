using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    [ForStream(Type = typeof (VideoStream))]
    [Filter(Name = "unsharp", MinInputs = 1, MaxInputs = 1)]
    // See https://ffmpeg.org/ffmpeg-filters.html#unsharp-1
    public class Unsharp : IFilter
    {
        public Unsharp(int lumaMSizeX = 5, int lumaMSizeY = 5, double lumaAmount = 0.0, int chromaMSizeX = 5,
            int chromaMSizeY = 5, double chromaAmount = 0.0, bool useOpenCL = false)
        {
            LumaMsizeX = lumaMSizeX;
            LumaMsizeY = lumaMSizeY;
            LumaAmount = lumaAmount;
            ChromaMsizeX = chromaMSizeX;
            ChromaMsizeY = chromaMSizeY;
            ChromaAmount = chromaAmount;
            OpenCL = useOpenCL;
        }
        public Unsharp(string command, bool useOpenCL = false)
        {
            OpenCL = useOpenCL;
            Command = command;
        }

        [FilterParameter(Name = "command", Default = null,ShouldHideName = true)]
        public string Command { get; set; }

        [FilterParameter(Name = "lx", Default = 5)]
        [Validate(LogicalOperators.GreaterThanOrEqual, 3)]
        [Validate(LogicalOperators.LesserThanOrEqual, 63)]
        public int? LumaMsizeX { get; set; }

        [Validate(LogicalOperators.GreaterThanOrEqual, 3)]
        [Validate(LogicalOperators.LesserThanOrEqual, 63)]
        [FilterParameter(Name = "ly", Default = 5)]
        public int? LumaMsizeY { get; set; }

        [Validate(LogicalOperators.GreaterThanOrEqual, -2.5)]
        [Validate(LogicalOperators.LesserThanOrEqual, 2.5)]
        [FilterParameter(Name = "la", Default = 0.0)]
        public double? LumaAmount { get; set; }

        [Validate(LogicalOperators.GreaterThanOrEqual, 3)]
        [Validate(LogicalOperators.LesserThanOrEqual, 63)]
        [FilterParameter(Name = "cx", Default = 5)]
        public int? ChromaMsizeX { get; set; }

        [Validate(LogicalOperators.GreaterThanOrEqual, 3)]
        [Validate(LogicalOperators.LesserThanOrEqual, 63)]
        [FilterParameter(Name = "cy", Default = 5)]
        public int? ChromaMsizeY { get; set; }

        [Validate(LogicalOperators.GreaterThanOrEqual, -2.5)]
        [Validate(LogicalOperators.LesserThanOrEqual, 2.5)]
        [FilterParameter(Name = "ca", Default = 0.0)]
        public double? ChromaAmount { get; set; }

        [FilterParameter(Name = "opencl", Default = false, Formatter = typeof (BoolToInt32Formatter))]
        public bool? OpenCL { get; set; }
    }
}