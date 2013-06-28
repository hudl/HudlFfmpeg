using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Filters.Templates
{
    public class Crossfade : Blend
    {
        private const string CrossfadeAlgorithm = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";

        public Crossfade(TimeSpan duration)
        {
            Duration = duration;
            Option = BlendVideoOptionTypes.all_expr;
        }

        private TimeSpan _duration; 
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value"); 
                }
                _duration = value; 
                Expression = string.Format(CrossfadeAlgorithm, value.TotalSeconds);
            }
        }
    }
}
