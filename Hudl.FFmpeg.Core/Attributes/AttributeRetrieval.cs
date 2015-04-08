using System;
using System.Collections.Generic;
using System.Linq;

namespace Hudl.FFmpeg.Attributes
{
    public class AttributeRetrieval
    {
        public static TAttribute GetAttribute<TAttribute>(Type itemType)
           where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(itemType).FirstOrDefault();
        }

        public static List<TAttribute> GetAttributes<TAttribute>(Type itemType)
            where TAttribute : Attribute
        {
            var allAttributes = itemType.GetCustomAttributes(true);
            if (allAttributes.Length == 0)
            {
                return null;
            }

            var matchingAttribute = allAttributes.OfType<TAttribute>().ToList();
            return matchingAttribute;
        }
    }
}
