﻿using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources
{
    [ContainsStream(Type = typeof(AudioStream))]
    [ContainsStream(Type = typeof(VideoStream))]
    public class Avi : BaseContainer
    {
        private const string FileFormat = ".avi";

        public Avi()
            : base(FileFormat)
        {
        }

        protected override IContainer Clone()
        {
            return new Avi();
        }
    }
}