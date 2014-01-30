using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Templates.BaseTypes
{
    /// <summary>
    /// This is the base template file for CommandFactory projects. This format will contain the necessary base functions for adding and assigning video, audio, and images.
    /// </summary>
    public abstract class BaseCommandFactoryTemplate
    {
        protected BaseCommandFactoryTemplate(CommandConfiguration configuration)
        {
            Factory = new CommandFactory(configuration);       
            AudioList = new List<string>();
            VideoList = new List<string>();
            ImageList = new List<string>();
        }

        public void AddAudio(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException("input");
            }
            AudioList.Add(input);
        }

        public void AddVideo(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException("input");
            }
            VideoList.Add(input);
        }

        public void AddImage(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException("input");
            }
            ImageList.Add(input);
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

        public List<IResource> GetAllWrittenFiles()
        {
            return Factory.GetAllOutput();
        }

        #region Internals
        internal protected bool SetupCompleted { get; protected set; }
        internal protected List<string> AudioList { get; protected set; }
        internal protected List<string> VideoList { get; protected set; }
        internal protected List<string> ImageList { get; protected set; }
        internal protected CommandFactory Factory { get; protected set; }
        #endregion 
    }
}
