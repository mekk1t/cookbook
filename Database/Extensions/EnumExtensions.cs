using System;

namespace KP.Cookbook.Database.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string str) =>
            (T)Enum.Parse(typeof(T), str);
    }
}
