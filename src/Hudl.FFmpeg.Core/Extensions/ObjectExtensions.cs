using System;
using System.Reflection;

namespace Hudl.FFmpeg.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNumeric(this object value)
        {
            return (value != null && value.GetType().IsNumeric());
        }

        public static bool IsNumeric(this Type type)
        {
            var typeCode  = Type.GetTypeCode(type);

            return (typeCode == TypeCode.Decimal ||
                    (type.GetTypeInfo().IsPrimitive && typeCode != TypeCode.Object && typeCode != TypeCode.Boolean &&
                     typeCode != TypeCode.Char));

        }
    }
}
