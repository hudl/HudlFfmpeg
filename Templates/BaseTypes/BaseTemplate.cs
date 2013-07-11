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
    /// This is the base template file for CommandFactory projects. This format will contain the necessary base functions for adding and assigning video, audio, and images.
    /// </summary>
    public abstract class BaseTemplate
    {
        protected BaseTemplate(CommandConfiguration configuration)
        {
            Factory = new CommandFactory(configuration);       
            AudioList = new List<IAudio>();
            VideoList = new List<IVideo>();
            ImageList = new List<IImage>();
        }

        public void Add(IAudio resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            AudioList.Add(resource);
        }
        
        public void Add(IVideo resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            VideoList.Add(resource);
        }

        public void Add(IImage resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            ImageList.Add(resource);
        }

        public List<IResource> Render()
        {
            if (!SetupCompleted)
            {
                SetupTemplate();
                SetupCompleted = true;
            }

            return Factory.Render();
        }

        public List<IResource> RenderWith<TProcessor>()
            where TProcessor : ICommandProcessor, new()
        {
            if (!SetupCompleted)
            {
                SetupTemplate();
                SetupCompleted = true;
            }

            return Factory.RenderWith<TProcessor>();
        }

        public List<IResource> RenderWith<TProcessor>(TProcessor processor)
            where TProcessor : ICommandProcessor, new()
        {
            if (!SetupCompleted)
            {
                SetupTemplate();
                SetupCompleted = true;
            }

            return Factory.RenderWith(processor);
        }

        /// <summary>
        /// Called right before the Render process
        /// </summary>
        protected abstract void SetupTemplate();

        #region Internals
        internal protected bool SetupCompleted { get; protected set; }
        internal protected List<IVideo> VideoList { get; protected set; }
        internal protected List<IAudio> AudioList { get; protected set; }
        internal protected List<IImage> ImageList { get; protected set; }
        internal protected CommandFactory Factory { get; protected set; }
        #endregion 
    }
}
