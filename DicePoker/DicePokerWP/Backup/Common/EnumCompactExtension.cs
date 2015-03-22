using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sanet.Common
{
    public static class EnumCompactExtension
    {
        public static T[] GetValues<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            IEnumerable<FieldInfo> fields = enumType.GetFields().Where(field => field.IsLiteral);
            return fields.Select(field => field.GetValue(enumType)).Select(value => (T)value).ToArray();
        }
    }
}
