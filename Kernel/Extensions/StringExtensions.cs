using System;

namespace KitProjects.MasterChef.Kernel.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool EqualsIgnoreCase(this string source, string toCheck)
        {
            return string.Equals(source, toCheck, StringComparison.OrdinalIgnoreCase);
        }
    }
}
