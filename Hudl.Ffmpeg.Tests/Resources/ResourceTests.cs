using System;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Tests.Assets;
using Xunit; 

namespace Hudl.Ffmpeg.Tests.Resources
{
    public class ResourceTests
    {
        [Fact]
        public void IVideo_CreateEmpty_SettingsSet()
        {
            var resource = ResourceFactory.CreateEmpty<Mp4>();

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.NotEmpty(resource.Name);
        }

        [Fact]
        public void IAudio_CreateEmpty_SettingsSet()
        {
            var resource = ResourceFactory.CreateEmpty<Mp3>();

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.NotEmpty(resource.Name);
        }

        [Fact]
        public void IImage_CreateEmpty_SettingsSet()
        {
            var resource = ResourceFactory.CreateEmpty<Png>();

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.NotEmpty(resource.Name);
        }

        [Fact]
        public void IVideo_CreateWithPath_SettingsSet()
        {
            const string path = "c:/source/";
            const string name = "apples.mp4";
            const string fullName = path + name;
            var resource = Resource.Create<Mp4>(fullName);

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.Equal(resource.Name, name);
            Assert.Equal(resource.Path, path);
            Assert.Equal(resource.FullName, fullName);
        }

        [Fact]
        public void IAudio_CreateWithPath_SettingsSet()
        {
            const string path = "c:/source/";
            const string name = "apples.mp3";
            const string fullName = path + name;
            var resource = Resource.Create<Mp3>(fullName);

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.Equal(resource.Name, name);
            Assert.Equal(resource.Path, path);
            Assert.Equal(resource.FullName, fullName);
        }

        [Fact]
        public void IImage_CreateWithPath_SettingsSet()
        {
            const string path = "c:/source/";
            const string name = "apples.png";
            const string fullName = path + name;
            var resource = Resource.Create<Png>(fullName);

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.Equal(resource.Name, name);
            Assert.Equal(resource.Path, path);
            Assert.Equal(resource.FullName, fullName);
        }

        [Fact]
        public void IVideo_CreateWithPathAndLength_SettingsSet()
        {
            const string path = "c:/source/";
            const string name = "apples.mp4";
            const string fullName = path + name;
            var mp4Length = TimeSpan.FromSeconds(212);
            var resource = Resource.Create<Mp4>(fullName, mp4Length);

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.Equal(resource.Name, name);
            Assert.Equal(resource.Path, path);
            Assert.Equal(resource.FullName, fullName);
            Assert.Equal(resource.Length, mp4Length);
        }

        [Fact]
        public void IAudio_CreateWithPathAndLength_SettingsSet()
        {
            const string path = "c:/source/";
            const string name = "apples.mp3";
            const string fullName = path + name;
            var mp3Length = TimeSpan.FromSeconds(212);
            var resource = Resource.Create<Mp3>(fullName, mp3Length);

            Assert.NotEmpty(resource.Id);
            Assert.NotEmpty(resource.Map);
            Assert.Equal(resource.Name, name);
            Assert.Equal(resource.Path, path);
            Assert.Equal(resource.FullName, fullName);
            Assert.Equal(resource.Length, mp3Length);
        }

        [Fact]
        public void IVideo_CreateWithBadPath_ThrowsException()
        {
            const string pngPathName = "c:/source/apples.png";
            
            Assert.Throws<ArgumentException>(() => Resource.Create<Mp4>(pngPathName));
        }

        [Fact]
        public void IAudio_CreateWithBadPath_ThrowsException()
        {
            const string pngPathName = "c:/source/apples.png";

            Assert.Throws<ArgumentException>(() => Resource.Create<Mp3>(pngPathName));
        }

        [Fact]
        public void IImage_CreateWithBadPath_ThrowsException()
        {
            const string pngPathName = "c:/source/apples.png";

            Assert.Throws<ArgumentException>(() => Resource.Create<Jpg>(pngPathName));
        }

        [Fact]
        public void Video_EncodedApplication_Load()
        {
            var resourceInfo = Resource.From(Utilities.GetVideoFile()).LoadMetadata();

            Assert.NotEmpty(resourceInfo.Info.EncodedApplication);
        }

        [Fact]
        public void Ts_FileLoad()
        {
            IResource resourceInfo = null; 
            
            Assert.DoesNotThrow(() => resourceInfo = Resource.From(@"c:\source\apple.ts"));
        }
        
        [Fact]
        public void Ismv_FileLoad()
        {
            IResource resourceInfo = null;

            Assert.DoesNotThrow(() => resourceInfo = Resource.From(@"c:\source\apple.ismv"));
        }


        private class ResourceFactory
        {
            public static TResource CreateEmpty<TResource>()
                where TResource : class, IResource, new()
            {
                return new TResource();
            }
        }
    }
}
