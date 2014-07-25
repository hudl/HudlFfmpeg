namespace Hudl.FFmpeg.Common
{
    /// <summary>
    /// the fade transition effect type applied in a Fade or AFade filter. 
    /// </summary>
    public enum FadeCurveType
    {
        /// <summary>
        /// select triangular, linear slope (default)
        /// </summary>
        Tri,

        /// <summary>
        /// select quarter of sine wave
        /// </summary>
        Qsin,

        /// <summary>
        /// select half of sine wave
        /// </summary>
        Hsin,

        /// <summary>
        /// select exponential sine wave
        /// </summary>
        Esin,

        /// <summary>
        /// select logarithmic
        /// </summary>
        Log,

        /// <summary>
        /// select inverted parabola
        /// </summary>
        Par,

        /// <summary>
        /// select quadratic
        /// </summary>
        Qua,

        /// <summary>
        /// select cubic
        /// </summary>
        Cub,

        /// <summary>
        /// select square root
        /// </summary>
        Squ,

        /// <summary>
        /// select cubic root
        /// </summary>
        Cbr
    }
}
