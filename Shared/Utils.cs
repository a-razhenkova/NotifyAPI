using System.Reflection;

namespace Shared
{
    public static class Utils
    {
        public static TAttribute GetRequiredCustomAttribute<TAttribute>(this object value)
            where TAttribute : Attribute
        {
            return value.GetType().GetRequiredCustomAttribute<TAttribute>();
        }

        public static TAttribute GetRequiredCustomAttribute<TAttribute>(this MemberInfo value)
            where TAttribute : Attribute
        {
            TAttribute? attribute = (TAttribute?)value.GetCustomAttribute(typeof(TAttribute), true);

            if (attribute == null)
                throw new ArgumentException($"Attribute of type '{typeof(TAttribute).Name}' not found for value '{value.Name}'.");

            return attribute;
        }
    }
}