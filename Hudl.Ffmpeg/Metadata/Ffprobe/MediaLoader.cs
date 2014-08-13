using System;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFmpeg.Metadata.FFprobe.Serializers;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe
{
    internal class MediaLoader
    {
        public MediaLoader(IContainer resource)
        {
            ReadInfo(resource);
        }

        public void ReadInfo(IContainer resource)
        {
            var ffprobeCommand = FFprobeCommand.Create(resource)
                                               .Register(new FFprobeStreamSerializer())
                                               .Register(new FFprobeFormatSerializer())
                                               .Execute();

            var ffprobeSerializerResult = FFprobeSerializer.Serialize(ffprobeCommand);

            HasAudio = ffprobeSerializerResult.GetCount(FFprobeCodecTypes.Audio) > 0;
            HasVideo = ffprobeSerializerResult.GetCount(FFprobeCodecTypes.Video) > 0;

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

        public void ReadVideo(FFprobeSerializerResult ffprobeSerializerResult)
        {
            VideoStream = FFprobeVideoStream.Create();

            int w, h;
            var fw = ffprobeSerializerResult.Get(FFprobeCodecTypes.Video, 0, "width") as FFprobeObject;
            var fh = ffprobeSerializerResult.Get(FFprobeCodecTypes.Video, 0, "height") as FFprobeObject;
            if (fw != null && int.TryParse(fw.Value.ToString(), out w)) VideoStream.Width = w;
            if (fh != null && int.TryParse(fh.Value.ToString(), out h)) VideoStream.Height = h;

            double d;
            var fd = ffprobeSerializerResult.Get(FFprobeCodecTypes.Video, 0, "duration") as FFprobeObject;
            if (fd != null && double.TryParse(fd.Value.ToString(), out d)) VideoStream.Duration = TimeSpan.FromSeconds(d);

            long br;
            var fbr = ffprobeSerializerResult.Get(FFprobeCodecTypes.Video, 0, "bit_rate") as FFprobeObject;
            if (fbr != null && long.TryParse(fbr.Value.ToString(), out br)) VideoStream.BitRate = br;

            var tb = ffprobeSerializerResult.Get(FFprobeCodecTypes.Video, 0, "time_base") as FFprobeFraction;
            if (tb != null) VideoStream.TimeBase = tb;

            var fr = ffprobeSerializerResult.Get(FFprobeCodecTypes.Video, 0, "avg_frame_rate") as FFprobeFraction;
            if (fr != null) VideoStream.FrameRate = fr;
        }

        public void ReadAudio(FFprobeSerializerResult ffprobeSerializerResult)
        {
            AudioStream = FFprobeAudioStream.Create();

            double d;
            var fd = ffprobeSerializerResult.Get(FFprobeCodecTypes.Audio, 0, "duration") as FFprobeObject;
            if (fd != null && double.TryParse(fd.Value.ToString(), out d)) AudioStream.Duration = TimeSpan.FromSeconds(d);

            long br;
            var fbr = ffprobeSerializerResult.Get(FFprobeCodecTypes.Audio, 0, "bit_rate") as FFprobeObject;
            if (fbr != null && long.TryParse(fbr.Value.ToString(), out br)) AudioStream.BitRate = br;

            var tb = ffprobeSerializerResult.Get(FFprobeCodecTypes.Audio, 0, "time_base") as FFprobeFraction;
            if (tb != null) AudioStream.TimeBase = tb;
        }

        public void ReadGeneral(FFprobeSerializerResult ffprobeSerializerResult)
        {
            var fea = ffprobeSerializerResult.GetFormat("TAG:encoder") as FFprobeObject;
            if (fea != null) EncodedApplication = fea.Value.ToString();
        }

        public bool HasVideo { get; protected set; }
        public bool HasAudio { get; protected set; }
        public string EncodedApplication { get; protected set; }
        public FFprobeVideoStream VideoStream { get; set; }
        public FFprobeAudioStream AudioStream { get; set; }
    }
}
