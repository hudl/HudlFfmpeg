using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;

namespace Hudl.FFmpeg.Filters.Templates
{
    public class CameraFlash : FilterchainTemplate
    {
        private readonly string Color;

        public CameraFlash(string color = null)
        {
            Color = color ?? "white";
        }

        public override List<StreamIdentifier> SetupTemplate(FFmpegCommand command, List<StreamIdentifier> streamIdList)
        {
            if (streamIdList.Count != 2)
            {
                throw new InvalidOperationException("CameraFlash Concatenate requires two input video streams.");
            }

            var streamTo = streamIdList[1];
            var streamFrom = streamIdList[0];
            var streamFromMetadata = MetadataHelpers.GetMetadataInfo(command, streamFrom);



            var fadeOut = command.Select(streamFrom)
                                  .Filter(Filterchain.FilterTo<VideoStream>(new Fade
                                  {
                                      TransitionType = FadeTransitionType.Out,
                                      StartFrame = streamFromMetadata.VideoStream.VideoMetadata.NumberOfFrames-5,
                                      NumberOfFrames = 5,
                                      Color = Color,
                                  }));
            var fadeIn = command.Select(streamTo)
                                 .Filter(Filterchain.FilterTo<VideoStream>(new Fade
                                 {
                                     TransitionType = FadeTransitionType.In,
                                     NumberOfFrames = 5,
                                     Color = Color,
                                 }));

            var result = command.Select(fadeOut.StreamIdentifiers)
                                .Select(fadeIn.StreamIdentifiers)
                                .Filter(Filterchain.FilterTo<VideoStream>(new Concat()));

            return result.StreamIdentifiers;
        }
    }
}
