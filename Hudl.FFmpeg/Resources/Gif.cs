﻿using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(VideoStream))]
    public class Gif : BaseContainer
    {
        private const string FileFormat = ".gif";

        public Gif()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Gif();
        }
    }
}
