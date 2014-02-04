using System.Collections.Generic;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Templates.BaseTypes
{
    /// <summary>
    /// This is the base template file for filterchains. This format will contain the necessary base functions for adding and assigning multiple chains for quick functionality.
    /// </summary>
    public abstract class BaseFilterchainTemplate
    {
        protected BaseFilterchainTemplate(IResource resourceToUse)
        {
            var resourceList = new List<IResource> {resourceToUse};
            Base = Filterchain.FilterTo(resourceList);
        }

        public static implicit operator Filterchain(BaseFilterchainTemplate filterchain)
        {
            return filterchain.Base;
        }

        #region Internals
        internal protected Filterchain Base { get; protected set; }
        #endregion
    }
}
