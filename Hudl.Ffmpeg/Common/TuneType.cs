namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// enumeration containing the known tune types
    /// You can optionally use -tune to change settings based upon the specifics of your input.
    /// Current tunings include: film, animation, grain, stillimage, psnr, ssim, fastdecode, zerolatency.
    /// For example, if your input is animation then use the animation tuning, or if you want to preserve grain then use the grain tuning.
    /// If you are unsure of what to use or your input does not match any of tunings then omit the -tune option.
    /// https://trac.ffmpeg.org/wiki/Encode/H.264#a2.Chooseapreset
    /// </summary>
    public enum TuneType
    {
        /// <summary>
        /// --deblock -1:-1 --psy-rd <unset>:0.15
        /// </summary>
        film,

        /// <summary>
        /// --bframes {+2} --deblock 1:1
        /// --psy-rd 0.4:<unset> --aq-strength 0.6
        /// --ref {Double if >1 else 1}
        /// </summary>
        animation,

        /// <summary>
        /// --aq-strength 0.5 --no-dct-decimate
        /// --deadzone-inter 6 --deadzone-intra 6
        /// --deblock -2:-2 --ipratio 1.1 
        /// --pbratio 1.1 --psy-rd <unset>:0.25
        /// --qcomp 0.8
        /// </summary>
        grain,

        /// <summary>
        /// --aq-strength 1.2 --deblock -3:-3
        /// --psy-rd 2.0:0.7
        /// </summary>
        stillimage,

        /// <summary>
        /// --aq-mode 0 --no-psy
        /// </summary>
        psnr,

        /// <summary>
        /// --aq-mode 2 --no-psy
        /// </summary>
        ssim,

        /// <summary>
        ///  --no-cabac --no-deblock --no-weightb
        /// --weightp 0
        /// </summary>
        fastdecode,

        /// <summary>
        /// --bframes 0 --force-cfr --no-mbtree
        /// --sync-lookahead 0 --sliced-threads
        /// --rc-lookahead 0
        /// </summary>
        zerolatency
    }
}
