using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.Ffmpeg.Filters.Templates
{
    public class Crossfade :
        Blend
    {
        private  const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))"; 

        public 


    }
}
