using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    public abstract class BaseBitRate : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-b";

        protected BaseBitRate(string suffix, int rate)
            : base(string.Format("{0}{1}", SettingType, suffix))
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public override void Validate()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Bit Rate must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Rate, "k");
        }

        public MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo)
        {
            infoToUpdate.BitRate = Rate * 1000;

            return infoToUpdate;
        }
    }
}
