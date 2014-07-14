using System;
using System.IO;
using MediaInfoLib;

namespace Hudl.Ffmpeg.Metadata.MediaInfo
{
    internal class MediaLoader
    {
        private readonly IMediaInfo _mi;

        internal MediaLoader(IMediaInfo mediaInfo)
        {
            _mi = mediaInfo;
        }

        public MediaLoader(string path)
            : this(new MediaInfoLib.MediaInfo())
        {
            ReadInfo(path);
        }

        public void ReadInfo(string path)
        {
            _mi.Option("Internet", "No");
            var opened = _mi.Open(path);
            if (opened == 0) throw new Exception("Unable to open file " + path);

            FullName = path;

            try
            {
                HasAudio = _mi.Count_Get(StreamKind.Audio) > 0;
                HasVideo = _mi.Count_Get(StreamKind.Video) > 0;
                HasImage = _mi.Count_Get(StreamKind.Image) > 0;

                if (HasAudio)
                {
                    LoadAudioData();
                }
                if (HasImage)
                {
                    LoadImageData();
                }
                if (HasVideo)
                {
                    LoadVideoData();
                }

                LoadCommonData();
               
            }
            finally
            {
                _mi.Close();
            }

            var fileInfo = new FileInfo(path);
            FileSize = fileInfo.Length;
        }

        public void LoadImageData()
        {
            int w, h;
            if (int.TryParse(_mi.Get(StreamKind.Image, 0, "Width"), out w)) Width = w;
            if (int.TryParse(_mi.Get(StreamKind.Image, 0, "Height"), out h)) Height = h;
        }

        public void LoadVideoData()
        {
            int w, h;
            if (int.TryParse(_mi.Get(StreamKind.Video, 0, "Width"), out w)) Width = w;
            if (int.TryParse(_mi.Get(StreamKind.Video, 0, "Height"), out h)) Height = h;

            double ar;
            if (double.TryParse(_mi.Get(StreamKind.Video, 0, "AspectRatio"), out ar)) AspectRatio = Math.Round(ar, 2);

            double fr;
            if (double.TryParse(_mi.Get(StreamKind.Video, 0, "FrameRate"), out fr)) FrameRate = fr;

            long br;
            if (long.TryParse(_mi.Get(StreamKind.Video, 0, "BitRate"), out br)) BitRate = br;
        }

        public void LoadAudioData()
        {
            //currently nothing special
        }

        public void LoadCommonData()
        {
            DateTime rd;
            RecordingDate = DateTime.TryParse(_mi.Get(StreamKind.General, 0, "Recorded_Date"), out rd) ? rd : File.GetLastWriteTime(FullName);

            // duration is in milliseconds
            double durms;
            if (double.TryParse(_mi.Get(StreamKind.General, 0, "Duration"), out durms)) Duration = TimeSpan.FromMilliseconds(durms);

            // encoder application
            var encodedApplication = _mi.Get(StreamKind.General, 0, "Encoded_Application");
            if (!string.IsNullOrWhiteSpace(encodedApplication)) EncodedApplication = encodedApplication; 
        }

        public string FullName { get; protected set; }
        public string EncodedApplication { get; protected set; }
        public TimeSpan Duration { get; protected set; }
        public bool HasVideo { get; protected set; }
        public bool HasAudio { get; protected set; }
        public bool HasImage { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public long BitRate { get; protected set; }
        public double AspectRatio { get; protected set; }
        public DateTime RecordingDate { get; protected set; }
        public double FrameRate { get; protected set; }
        public long FileSize { get; protected set; }
    }
}
