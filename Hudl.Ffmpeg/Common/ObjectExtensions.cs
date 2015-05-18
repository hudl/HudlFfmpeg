using System;

namespace Hudl.FFmpeg.Common
{
    public static class ObjectExtensions
    {
        public static bool IsNumeric(this object obj)
        {
            return (obj != null) && IsNumeric(obj.GetType());
        }

        private static bool IsNumeric(Type type)
        {
            if (type == null)
            {
                return false;
            }

            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }

            return false;
        }
    }
}
