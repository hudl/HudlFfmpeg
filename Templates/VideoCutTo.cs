using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Templates.BaseTypes;

namespace Hudl.Ffmpeg.Templates
{
    public class VideoCutTo : BaseFilterchainTemplate
    {
        private VideoCutTo(IResource resourceToUse, double startTime, double endTime)
            : base(resourceToUse)
        {
            End = endTime;
            Start = startTime;
            Duration = endTime - startTime;
            TimebaseUnit = VideoUnitType.Seconds;
            
            SetUpTemplate();
        }
        private VideoCutTo(IResource resourceToUse, double startFrame, double endFrame, double duration)
            : base(resourceToUse)
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

        public static VideoCutTo Create<TResource>(double startTime, double endTime)
            where TResource : IVideo, new()
        {
            return Create(new TResource(), startTime, endTime);
        }
        public static VideoCutTo Create<TResource>(double startFrame, double endFrame, double duration)
          where TResource : IVideo, new()
        {
            return Create(new TResource(), startFrame, endFrame, duration);
        }
        public static VideoCutTo Create(IResource resourceToUse, double startTime, double endTime)
        {
            if (resourceToUse == null)
            {
                throw new ArgumentNullException("resourceToUse");
            }

            return new VideoCutTo(resourceToUse, startTime, endTime);
        }
        public static VideoCutTo Create(IResource resourceToUse, double startFrame, double endFrame, double duration)
        {
            if (resourceToUse == null)
            {
                throw new ArgumentNullException("resourceToUse");
            }

            return new VideoCutTo(resourceToUse, startFrame, endFrame, duration);
        }
    }
}
