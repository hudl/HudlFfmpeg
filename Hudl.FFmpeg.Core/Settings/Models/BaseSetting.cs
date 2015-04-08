using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings.Models
{
    /// <summary>
    /// The base implementation of Setting's, will store the basic information on setup, and makes it accessible to parents.
    /// </summary>
    public abstract class BaseSetting : ISetting
    {
        protected BaseSetting(string type)
        {
            Type = type;
        }
        
        /// <summary>
        /// Defines the settings type, name that is given to ffmpeg
        /// </summary>
        public string Type { get; protected set; }

        public virtual void Validate()
        {
        }

        public string GetAndValidateString()
        {
            Validate();

            return ToString(); 
        }
    }
}
