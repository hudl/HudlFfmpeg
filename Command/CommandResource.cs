using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandResource<TResource>
        where TResource : IResource
    {
        internal CommandResource(TResource resource)
            : this(new SettingsCollection(), resource)
        {
        }
        internal CommandResource(SettingsCollection settings, TResource resource)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            Resource = resource;
            Settings = settings; 
        }

        public TResource Resource { get; set; }

        public SettingsCollection Settings { get; set; }
    }
}
