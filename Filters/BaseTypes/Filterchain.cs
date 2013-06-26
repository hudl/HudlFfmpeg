using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    public class Filterchain<TOutput>
        where TOutput : IResource, new()
    {
        public Filterchain()
        {
            Output = new TOutput(); 
        }
        public Filterchain(params IFilter[] filters) : this()
        {
            Filters.AddRange(filters); 
        }
        public Filterchain(TOutput outputToUse) 
        {
            Output = outputToUse;
        }
        public Filterchain(TOutput outputToUse, params IFilter[] filters) : this(outputToUse)
        {
            Filters.AddRange(filters); 
        }

        public TOutput Output { get; protected set; }

        public AppliesToCollecion<IFilter, TOutput> Filters { get; protected set; }

        public override string ToString() 
        {
            //perform simple validation on the filter chain
            if (Filters.Count == 0)
            {
                throw new ArgumentException("Filterchain must contain at least one filter.");
            }

            //process all the filters in the filter chain
            var filterChain = new StringBuilder(100); 
            foreach(var filter in Filters.Items) 
            {
                if (filterChain.Length > 0) filterChain.Append(",");
                filterChain.Append(filter.ToString())
                           .Append(" "); 
            }

            //return the filter chain command
            return filterChain.ToString();
        }

        public static Filterchain<TAsOutput> ToOutput<TAsOutput>()
            where TAsOutput : IResource, new()
        {
            return new Filterchain<TAsOutput>();
        }

        public static Filterchain<TAsOutput> ToOutput<TAsOutput>(params IFilter[] filters)
            where TAsOutput : IResource, new()
        {
            return new Filterchain<TAsOutput>(filters);
        }

        public static Filterchain<TAsOutput> ToOutput<TAsOutput>(TAsOutput outputToUse)
            where TAsOutput : IResource, new()
        {
            return new Filterchain<TAsOutput>(outputToUse);
        }

        public static Filterchain<TAsOutput> ToOutput<TAsOutput>(TAsOutput outputToUse, params IFilter[] filters)
            where TAsOutput : IResource, new()
        {
            return new Filterchain<TAsOutput>(outputToUse, filters);
        }
}
