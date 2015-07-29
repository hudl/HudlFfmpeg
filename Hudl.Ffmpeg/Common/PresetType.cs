namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// enumeration containing the known preset types
    /// The general guideline is to use the slowest preset that you have patience for.
    /// Current presets in descending order of speed are: ultrafast,superfast, veryfast, faster, fast, medium, slow, slower, veryslow, placebo.
    /// The default preset is medium. Ignore placebo as it is not useful (see https://trac.ffmpeg.org/wiki/Encode/H.264#FAQ).
    /// You can see a list of current presets with -preset help (see example below), and what settings they apply with x264 --fullhelp.
    /// https://trac.ffmpeg.org/wiki/Encode/H.264#a2.Chooseapreset
    /// </summary>
    public enum PresetType
    {
        ultrafast,
        superfast,
        veryfast,
        faster,
        fast,
        medium,
        slow,
        slower,
        veryslow,
        placebo
    }
}
