using System;
using System.Linq;
using System.Reflection;
using System.Collections;

using Fasterflect;

using AtlasLib.Utils;

namespace AtlasLib.Reflection
{
    public static class ReflectUtils
    {
        private static readonly MethodInfo GET_RAW_BYTES_METHOD_INFO = typeof(Assembly).GetMethod("GetRawBytes", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInvoker GetRawBytesMethod = GetDelegate(GET_RAW_BYTES_METHOD_INFO);

        public static readonly TypeCode[] SimpleTypes = new TypeCode[] { TypeCode.Boolean, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16,
                                                                          TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32, TypeCode.Int64,
                                                                          TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal,
                                                                          TypeCode.DateTime, TypeCode.Char, TypeCode.String };

        public const TypeCode NULL_TYPE_CODE = TypeCode.Empty;

        public static object Instantiate(Type type, params object[] parameters)
        {
            Assert.AllNotNull(parameters);

            return type.CreateInstance(parameters);
        }

        public static MethodInvoker GetDelegate(MethodInfo methodInfo) 
        {
            return methodInfo.DelegateForCallMethod();
        }

        public static byte[] ToBytes(this Assembly assembly)
        {
            return GetRawBytesMethod.Invoke(assembly) as byte[];
        }

        public static T ConvertTo<T>(this object obj)
        {
            Assert.NotNull(obj);
            Assert.IsType<T>(obj);

            return (T)obj;
        }

        public static bool IsNull(object obj)
        {
            return obj == null || Type.GetTypeCode(obj?.GetType()) == NULL_TYPE_CODE;
        }

        public static bool IsSimple(this Type type)
        {
            return SimpleTypes.Contains(Type.GetTypeCode(type)) || type.IsEnum;
        }

        public static TypeCode[] GetSimpleTypes()
        {
            TypeCode[] array = new TypeCode[SimpleTypes.Length];

            Array.Copy(SimpleTypes, array, SimpleTypes.Length);

            return array;
        }

        public static bool IsList(this Type type)
        {
            return type == typeof(IEnumerable) || type.IsAssignableFrom(typeof(IEnumerable));
        }

        public static bool IsDictionary(this Type type)
        {
            return type == typeof(IDictionary) || type.IsAssignableFrom(typeof(IDictionary));
        }

        public static bool IsArray(this Type type)
        {
            return type.IsArray;
        }
    }
}
