namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// interface that forces a type to expose it's Settings interface 
    /// </summary>
    public interface ISettingable
    {
        SettingsCollection Settings { get; set; }
    }
}
