using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe.Serializers
{
    internal class FFprobeGeneralSerializer
    {
        public static List<FFprobeKeyValuePair> Serialize(string ffprobeOutput)
        {
            var ffprobeSerialized = new List<FFprobeKeyValuePair>();

            var ffprobeOutputLines = ffprobeOutput.Split(new [] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var ffprobeLine in ffprobeOutputLines)
            {
                var indexOfValueSplit = ffprobeLine.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                if (indexOfValueSplit == -1)
                {
                    continue;
                }

                var ffprobeValuePairKey = ffprobeLine.Substring(0, indexOfValueSplit);
                var ffprobeValuePairValue = ffprobeLine.Substring(indexOfValueSplit + 1);
                if (string.IsNullOrWhiteSpace(ffprobeValuePairKey) || string.IsNullOrWhiteSpace(ffprobeValuePairValue))
                {
                    continue; 
                }

                ffprobeSerialized.Add(FFprobeKeyValuePair.Create(ffprobeValuePairKey.Trim(), ffprobeValuePairValue.Trim()));
            }

            return ffprobeSerialized;
        }

        public static FFprobeKeyValuePair SerializeAsFFprobeFraction(List<FFprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FFprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }
            
            FFprobeFraction ffprobeFraction;

            if (!FFprobeFraction.TryParse(ffprobeObject, out ffprobeFraction))
            {
                return null; 
            }

            return FFprobeKeyValuePair.Create(key, ffprobeFraction);
        }

        public static FFprobeKeyValuePair SerializeAsFFprobeInt(List<FFprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FFprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            int value;

            if (!int.TryParse(ffprobeObject.Value.ToString(), out value))
            {
                return null;
            }

            return FFprobeKeyValuePair.Create(key, FFprobeObject.Create(value));
        }

        public static FFprobeKeyValuePair SerializeAsFFprobeLong(List<FFprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FFprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            long value;

            if (!long.TryParse(ffprobeObject.Value.ToString(), out value))
            {
                return null;
            }

            return FFprobeKeyValuePair.Create(key, FFprobeObject.Create(value));
        }

        public static FFprobeKeyValuePair SerializeAsFFprobeDouble(List<FFprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FFprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            double value;

            if (!double.TryParse(ffprobeObject.Value.ToString(), out value))
            {
                return null;
            }

            return FFprobeKeyValuePair.Create(key, FFprobeObject.Create(value));
        }

        public static FFprobeKeyValuePair SerializeAsFFprobeString(List<FFprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FFprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            return FFprobeKeyValuePair.Create(key, FFprobeObject.Create(ffprobeObject.Value.ToString()));
        }

    }
}
