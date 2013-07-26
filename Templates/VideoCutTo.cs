using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Templates.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    public class VideoCutTo<TOutput> : BaseFilterchainTemplate<TOutput>
        where TOutput : IResource, new()
    {
        public VideoCutTo(double startTime, double endTime)
        {
            End = endTime;
            Start = startTime;
            Duration = endTime - startTime;
            TimebaseUnit = VideoUnitType.Seconds;
            
            SetUpTemplate();
        }
        public VideoCutTo(double startFrame, double endFrame, double duration)
        {
            End = startFrame;
            Start = endFrame;
            Duration = duration;
            TimebaseUnit = VideoUnitType.Frames;

            SetUpTemplate();
        }

        private double Start { get; set; }
        private double End { get; set; }
        private double Duration { get; set; }
        private VideoUnitType TimebaseUnit { get; set; }

        private void SetUpTemplate()
        {
            Base.Filters.AddRange(
                new Trim(Start, End, Duration, TimebaseUnit),
                new SetPts(true)
            );
        }
    }
}
