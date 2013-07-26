using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Filters.Templates;
using Hudl.Ffmpeg.Resolution;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Templates.BaseTypes
{
    /// <summary>
    /// This is the base template file for filterchains. This format will contain the necessary base functions for adding and assigning multiple chains for quick functionality.
    /// </summary>
    public abstract class BaseFilterchainTemplate<TOutput>
        where TOutput : IResource, new()
    {
        protected BaseFilterchainTemplate()
        {
            Base = Filterchain.FilterTo<TOutput>();
        }

        public static implicit operator Filterchain<TOutput>(BaseFilterchainTemplate<TOutput> filterchain)
        {
            return filterchain.Base;
        }

        public static implicit operator Filterchain<IResource>(BaseFilterchainTemplate<TOutput> filterchain)
        {
            return filterchain.Base;
        }

        #region Internals
        internal protected Filterchain<TOutput> Base { get; protected set; }
        #endregion
    }
}
