using System;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;
using Hudl.Ffmpeg.Metadata.Ffprobe.Serializers;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe
{
    internal class MediaLoader
    {
        public MediaLoader(IContainer resource)
        {
            ReadInfo(resource);
        }

        public void ReadInfo(IContainer resource)
        {
            var ffprobeCommand = FfprobeCommand.Create(resource)
                                               .Register(new FfprobeStreamSerializer())
                                               .Register(new FfprobeFormatSerializer())
                                               .Execute();

            var ffprobeSerializerResult = FfprobeSerializer.Serialize(ffprobeCommand);

            HasAudio = ffprobeSerializerResult.GetCount(FfprobeCodecTypes.Audio) > 0;
            HasVideo = ffprobeSerializerResult.GetCount(FfprobeCodecTypes.Video) > 0;

            ReadGeneral(ffprobeSerializerResult);

            if (HasAudio)
            {
                ReadAudio(ffprobeSerializerResult);
            }

            if (HasVideo)
            {
                ReadVideo(ffprobeSerializerResult);
            }
        }

        public void ReadVideo(FfprobeSerializerResult ffprobeSerializerResult)
        {
            VideoStream = FfprobeVideoStream.Create();

            int w, h;
            var fw = ffprobeSerializerResult.Get(FfprobeCodecTypes.Video, 0, "width") as FfprobeObject;
            var fh = ffprobeSerializerResult.Get(FfprobeCodecTypes.Video, 0, "height") as FfprobeObject;
            if (fw != null && int.TryParse(fw.Value.ToString(), out w)) VideoStream.Width = w;
            if (fh != null && int.TryParse(fh.Value.ToString(), out h)) VideoStream.Height = h;

            double d;
            var fd = ffprobeSerializerResult.Get(FfprobeCodecTypes.Video, 0, "duration") as FfprobeObject;
            if (fd != null && double.TryParse(fd.Value.ToString(), out d)) VideoStream.Duration = TimeSpan.FromSeconds(d);

            long br;
            var fbr = ffprobeSerializerResult.Get(FfprobeCodecTypes.Video, 0, "bit_rate") as FfprobeObject;
            if (fbr != null && long.TryParse(fbr.Value.ToString(), out br)) VideoStream.BitRate = br;

            var tb = ffprobeSerializerResult.Get(FfprobeCodecTypes.Video, 0, "time_base") as FfprobeFraction;
            if (tb != null) VideoStream.TimeBase = tb;

            var fr = ffprobeSerializerResult.Get(FfprobeCodecTypes.Video, 0, "avg_frame_rate") as FfprobeFraction;
            if (fr != null) VideoStream.FrameRate = fr;
        }

        public void ReadAudio(FfprobeSerializerResult ffprobeSerializerResult)
        {
            AudioStream = FfprobeAudioStream.Create();

            double d;
            var fd = ffprobeSerializerResult.Get(FfprobeCodecTypes.Audio, 0, "duration") as FfprobeObject;
            if (fd != null && double.TryParse(fd.Value.ToString(), out d)) AudioStream.Duration = TimeSpan.FromSeconds(d);

            long br;
            var fbr = ffprobeSerializerResult.Get(FfprobeCodecTypes.Audio, 0, "bit_rate") as FfprobeObject;
            if (fbr != null && long.TryParse(fbr.Value.ToString(), out br)) AudioStream.BitRate = br;

            var tb = ffprobeSerializerResult.Get(FfprobeCodecTypes.Audio, 0, "time_base") as FfprobeFraction;
            if (tb != null) AudioStream.TimeBase = tb;
        }

        public void ReadGeneral(FfprobeSerializerResult ffprobeSerializerResult)
        {
            var fea = ffprobeSerializerResult.GetFormat("TAG:encoder") as FfprobeObject;
            if (fea != null) EncodedApplication = fea.Value.ToString();
        }

        public bool HasVideo { get; protected set; }
        public bool HasAudio { get; protected set; }
        public string EncodedApplication { get; protected set; }
        public FfprobeVideoStream VideoStream { get; set; }
        public FfprobeAudioStream AudioStream { get; set; }
    }
}
