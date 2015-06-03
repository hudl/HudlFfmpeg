using System;
using System.Collections.Generic;
using System.Drawing;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Extensions;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Common
{
    /// <summary>
    /// helper class that helps with validation of objects in a ffmpeg project
    /// </summary>
    internal class Helpers
    {
        /// <summary>
        /// returns a list of scaling presets for ffmpeg.
        /// </summary>
        public static Dictionary<ScalePresetType, Size> ScalingPresets
        {
            get
            {
                return new Dictionary<ScalePresetType, Size>
                    {
                        {ScalePresetType.Svga, new Size(800, 600)},
                        {ScalePresetType.Xga, new Size(1024, 768)},
                        {ScalePresetType.Ega, new Size(640, 350)},
                        {ScalePresetType.Sd240, new Size(432, 240)},
                        {ScalePresetType.Sd360, new Size(640, 360)},
                        {ScalePresetType.Sd480, new Size(852, 480)},
                        {ScalePresetType.Hd720, new Size(1280, 720)},
                        {ScalePresetType.Hd1080, new Size(1920, 1080)}
                    };
            }
        }

        /// <summary>
        /// generates a new resource map variable
        /// </summary>
        /// <returns></returns>
        public static string NewMap()
        {
            return string.Concat("mp", Guid.NewGuid().ToString().Substring(0, 8));
        }

        /// <summary>
        /// gets the path/domain from a full name base
        /// </summary>
        public static string GetPathFromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty", "fullName");
            }

            var fullNameNormalized = fullName.Replace("\\", "/");
            var fullNameFinalIndexOf = fullNameNormalized.LastIndexOf("/", StringComparison.Ordinal);
            return fullNameFinalIndexOf == -1
                ? string.Empty
                : fullNameNormalized.Substring(0, fullNameFinalIndexOf + 1);
        }

        /// <summary>
        /// gets the name from a full name base
        /// </summary>
        public static string GetNameFromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty", "fullName");
            }

            var fullNameNormalized = fullName.Replace("\\", "/");
            var fullNameFinalIndexOf = fullNameNormalized.LastIndexOf("/", StringComparison.Ordinal);
            return fullNameFinalIndexOf == -1
                ? string.Empty
                : fullNameNormalized.Substring(fullNameFinalIndexOf + 1);
        }

        /// <summary>
        /// gets the extension from a full name base
        /// </summary>
        public static string GetExtensionFromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty", "fullName");
            }

            var queryStringIndex = fullName.IndexOf("?", StringComparison.Ordinal);
            if (queryStringIndex != -1)
            {
                fullName = fullName.Substring(0, queryStringIndex);
            }

            var finalIndex = fullName.LastIndexOf(".", StringComparison.Ordinal);
            if (finalIndex != -1)
            {
                fullName = fullName.Substring(finalIndex + 1);
            }

            return fullName;
        }

        /// <summary>
        /// escapes the path of the provided resource.
        /// </summary>
        public static string EscapePath(IContainer resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return string.Format("\"{0}\"",
                                 resource.FullName.Replace('\\', '/'));
        }
       
        /// <summary>
        /// Breaks does command streamIds into divisable subsets that then can be used to apply filters in chunks that ffmpeg will accept. while still abstracting from the user.
        /// </summary>
        public static List<StreamIdentifier[]> BreakStreamIdentifiers(int division, params StreamIdentifier[] streamIds)
        {
            if (streamIds == null)
            {
                throw new ArgumentNullException("streamIds");
            }

            var index = 0;
            var subDivision = division - 1;
            var breakouts = new List<StreamIdentifier[]>();
            var resourcesRemainderCount = streamIds.Length;
            resourcesRemainderCount -= (resourcesRemainderCount > division)
                                            ? division
                                            : streamIds.Length;
            breakouts.Add(streamIds.SubArray(0, division));
            while (resourcesRemainderCount > 0)
            {
                index++;
                var length = (resourcesRemainderCount > subDivision)
                                    ? subDivision
                                    : resourcesRemainderCount;
                resourcesRemainderCount -= length;
                breakouts.Add(streamIds.SubArray(1 + (index * subDivision), length));
            }

            return breakouts;
        }
    }
}
