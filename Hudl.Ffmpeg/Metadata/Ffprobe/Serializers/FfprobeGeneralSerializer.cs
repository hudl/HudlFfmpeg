using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe.Serializers
{
    internal class FfprobeGeneralSerializer
    {
        public static List<FfprobeKeyValuePair> Serialize(string ffprobeOutput)
        {
            var ffprobeSerialized = new List<FfprobeKeyValuePair>();

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

                ffprobeSerialized.Add(FfprobeKeyValuePair.Create(ffprobeValuePairKey.Trim(), ffprobeValuePairValue.Trim()));
            }

            return ffprobeSerialized;
        }

        public static FfprobeKeyValuePair SerializeAsFfprobeFraction(List<FfprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FfprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }
            
            FfprobeFraction ffprobeFraction;

            if (!FfprobeFraction.TryParse(ffprobeObject, out ffprobeFraction))
            {
                return null; 
            }

            return FfprobeKeyValuePair.Create(key, ffprobeFraction);
        }

        public static FfprobeKeyValuePair SerializeAsFfprobeInt(List<FfprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FfprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            int value;

            if (!int.TryParse(ffprobeObject.Value.ToString(), out value))
            {
                return null;
            }

            return FfprobeKeyValuePair.Create(key, FfprobeObject.Create(value));
        }

        public static FfprobeKeyValuePair SerializeAsFfprobeLong(List<FfprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FfprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            long value;

            if (!long.TryParse(ffprobeObject.Value.ToString(), out value))
            {
                return null;
            }

            return FfprobeKeyValuePair.Create(key, FfprobeObject.Create(value));
        }

        public static FfprobeKeyValuePair SerializeAsFfprobeDouble(List<FfprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FfprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            double value;

            if (!double.TryParse(ffprobeObject.Value.ToString(), out value))
            {
                return null;
            }

            return FfprobeKeyValuePair.Create(key, FfprobeObject.Create(value));
        }

        public static FfprobeKeyValuePair SerializeAsFfprobeString(List<FfprobeKeyValuePair> inputValuePairs, string key)
        {
            var inputValuePair = inputValuePairs.FirstOrDefault(ivp => ivp.Key == key);
            if (inputValuePair == null)
            {
                return null;
            }

            var ffprobeObject = inputValuePair.Value as FfprobeObject;
            if (ffprobeObject == null)
            {
                return null;
            }

            return FfprobeKeyValuePair.Create(key, FfprobeObject.Create(ffprobeObject.Value.ToString()));
        }

    }
}
