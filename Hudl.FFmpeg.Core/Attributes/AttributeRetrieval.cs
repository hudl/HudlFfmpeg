using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hudl.FFmpeg.Attributes
{
    public class AttributeRetrieval
    {
        public static TAttribute GetAttribute<TAttribute>(Type itemType)
           where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(itemType, typeof(TAttribute));
        }

        public static TAttribute GetAttribute<TAttribute>(PropertyInfo itemType)
          where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(itemType, typeof(TAttribute));
        }

        public static TAttribute GetAttribute<TAttribute>(MemberInfo itemType)
         where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(itemType, typeof(TAttribute));
        }

        public static List<TAttribute> GetAttributes<TAttribute>(Type itemType)
            where TAttribute : Attribute
        {
            var allAttributes = Attribute.GetCustomAttributes(itemType, typeof(TAttribute));
            if (allAttributes.Length == 0)
            {
                return null;
            }

            var matchingAttribute = allAttributes.OfType<TAttribute>().ToList();
            return matchingAttribute;
        }
     
        public static List<TAttribute> GetAttributes<TAttribute>(PropertyInfo itemType)
            where TAttribute : Attribute
        {
            var allAttributes = Attribute.GetCustomAttributes(itemType, typeof(TAttribute));
            if (allAttributes.Length == 0)
            {
                return null;
            }

            var matchingAttribute = allAttributes.OfType<TAttribute>().ToList();
            return matchingAttribute;
        }

        public static List<TAttribute> GetAttributes<TAttribute>(MemberInfo itemType)
          where TAttribute : Attribute
        {
            var allAttributes = Attribute.GetCustomAttributes(itemType, typeof(TAttribute));
            if (allAttributes.Length == 0)
            {
                return null;
            }

            var matchingAttribute = allAttributes.OfType<TAttribute>().ToList();
            return matchingAttribute;
        }
    }
}
