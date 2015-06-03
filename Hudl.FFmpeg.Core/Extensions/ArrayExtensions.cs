using System;

namespace Hudl.FFmpeg.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            if ((index + length) > data.Length)
            {
                length = data.Length - index; 
            }
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
