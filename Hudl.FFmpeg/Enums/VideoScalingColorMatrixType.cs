namespace Hudl.FFmpeg.Enums
{
    public enum VideoScalingColorMatrixType
    {
        /// <summary>
        /// allow the color matrix to be detected automatically (default) 
        /// </summary>
        Auto = 0, 
        /// <summary>
        /// format conforming to International Telecommunication Union (ITU) Recommendation BT.709.
        /// </summary>
        Bt709 = 1, 
        /// <summary>
        /// set color space conforming to the United States Federal Communications Commission (FCC) Code of Federal Regulations (CFR) Title 47 (2003) 73.682 (a).
        /// </summary>
        Fcc = 2, 
        /// <summary>
        /// set color space conforming to:
        ///  - ITU Radiocommunication Sector (ITU-R) Recommendation BT.601
        ///  - ITU-R Rec. BT.470-6 (1998) Systems B, B1, and G
        ///  - Society of Motion Picture and Television Engineers (SMPTE) ST 170:2004
        /// </summary>
        Bt601 = 3, 
        /// <summary>
        /// set color space conforming to SMPTE ST 240:1999.
        /// </summary>
        Smpte240M = 4
    }
}
