using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class Filterchain
    {
        /// <summary>
        /// Returns a new instance of the filterchain
        /// </summary>
        public static Filterchainv2 FilterTo<TResource>(int count, params IFilter[] filters)
            where TResource : IResource, new()
        {
            var outputList = new List<IResource>();
            for (var i = 0; i < count; i++)
            {
                outputList.Add(new TResource());
            }
            return FilterTo(outputList, filters);
        }

        /// <summary>
        /// Returns a new instance of the filterchain
        /// </summary>
        public static Filterchainv2 FilterTo(List<IResource> outputsToUse, params IFilter[] filters)
        {
            if (outputsToUse == null || outputsToUse.Count == 0)
            {
                throw new ArgumentException("Outputs specified cannot be null or empty.", "outputsToUse");
            }

            return Filterchainv2.Create(outputsToUse, filters);
        }
    }
}
