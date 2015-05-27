using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Attributes
{
    public class AttributeValidation
    {
        /// <summary>
        /// returns a boolean indicating if <cref name="TObject"/> is applicable to <cref name="TRestrictedTo"/> 
        /// </summary>
        /// <typeparam name="TCompareFrom">the type in question to be applied to</typeparam>
        /// <typeparam name="TCompareTo">the type in question that is required</typeparam>
        public static bool AttributeTypeEquals<TCompareFrom, TCompareTo>()
            where TCompareTo : IStream
        {
            var compareToObjectType = typeof(TCompareTo);
            var compareFromObjectType = typeof(TCompareFrom);
            return AttributeTypeEquals(compareFromObjectType, compareToObjectType);
        }

        /// <summary>
        /// returns a boolean indicating if <cref name="objectType"/> is applicable to <cref name="restrictedType"/> 
        /// </summary>
        public static bool AttributeTypeEquals(Type compareFromObjectType, Type compareToObjectType)
        {
            var matchingAttributes = AttributeRetrieval.GetAttributes<ForStreamAttribute>(compareFromObjectType);
            if (matchingAttributes.Count == 0)
            {
                return false;
            }

            return matchingAttributes.Any(attribute => (attribute.Type == compareToObjectType ||
                                                        attribute.Type.IsAssignableFrom(compareToObjectType)));
        }
    }
}
